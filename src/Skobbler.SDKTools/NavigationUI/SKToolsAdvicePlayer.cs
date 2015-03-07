using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using JavaObject = Java.Lang.Object;
using Android.Media;
using Java.IO;
using Console = System.Console;
using Skobbler.Ngx.Navigation;
using Android.Util;

namespace Skobbler.Ngx.SDKTools.NavigationUI
{
    public class SKToolsAdvicePlayer : JavaObject, MediaPlayer.IOnCompletionListener, MediaPlayer.IOnErrorListener
    {

        private const string TAG = "SKToolsAdvicePlayer";

        // constants for advice priority - user requested advices have the highest,
        // speed warnings the lowest
        public const int PRIORITY_USER = 0;

        public const int PRIORITY_NAVIGATION = 1;

        public const int PRIORITY_SPEED_WARNING = 2;

        /// <summary>
        /// The singleton instance of the advice player.
        /// </summary>
        private static SKToolsAdvicePlayer instance;

        /// <summary>
        /// The single player.
        /// </summary>
        private MediaPlayer player;

        /// <summary>
        /// The temporary file for storing the current advice
        /// </summary>
        private string tempAdviceFile = null;

        /// <summary>
        /// Queued advice that will be played after the player finishes playing the
        /// current advice.
        /// </summary>
        private string[] nextAdvice;

        /// <summary>
        /// The priority of the queued advice.
        /// </summary>
        private int nextAdvicePriority;

        /// <summary>
        /// Indicates if the user has chosen to mute the advices.
        /// </summary>
        private bool isMuted;

        /// <summary>
        /// Indicates whether the player is busy playing an advice.
        /// </summary>
        private bool isBusy;

        public static SKToolsAdvicePlayer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SKToolsAdvicePlayer();
                }
                return instance;
            }
        }

        private SKToolsAdvicePlayer()
        {
            player = new MediaPlayer();
            player.SetAudioStreamType(Stream.Music);
            player.SetOnCompletionListener(this);
            player.SetOnErrorListener(this);
        }

        /// <summary>
        /// method that retrieves the current volume level of the device audio
        /// manager with stream type STREAM_MUSIC </summary>
        /// <param name="activity">
        /// @return </param>
        public static int getCurrentDeviceVolume(Activity activity)
        {
            AudioManager audioManager = (AudioManager)activity.GetSystemService(Context.AudioService);
            return audioManager.GetStreamVolume(Stream.Music);
        }

        /// <summary>
        /// method that retrieves the maximum volume level of the device audio
        /// manager with the stream type STREAM_MUSIC </summary>
        /// <param name="activity"> - the current activity
        /// @return </param>
        public static int getMaximAudioLevel(Activity activity)
        {
            AudioManager audioManager = (AudioManager)activity.GetSystemService(Context.AudioService);
            return audioManager.GetStreamMaxVolume(Stream.Music);
        }

        public virtual void enableMute()
        {
            isMuted = true;
        }

        public virtual void disableMute()
        {
            isMuted = false;
        }

        public virtual bool Muted
        {
            get
            {
                return isMuted;
            }
        }

        /// <summary>
        /// Plays an advice. The individual sound files to play are contained in an
        /// array list. </summary>
        /// <param name="adviceParts"> an array list of sound file names </param>
        public virtual void playAdvice(string[] adviceParts, int priority)
        {
            if (isMuted || adviceParts == null)
            {
                return;
            }

            if (isBusy)
            {
                if (nextAdvice == null || (priority <= nextAdvicePriority))
                {
                    nextAdvice = adviceParts;
                    nextAdvicePriority = priority;
                }
                return;
            }

            SKAdvisorSettings advisorSettings = SKMaps.Instance.MapInitSettings.AdvisorSettings;
            string soundFilesDirPath = advisorSettings.ResourcePath + advisorSettings.Language.Value + "/sound_files/";

            tempAdviceFile = soundFilesDirPath + "temp.mp3";
            bool validTokensFound = false;
            ByteArrayOutputStream stream = new ByteArrayOutputStream();
            for (int i = 0; i < adviceParts.Length; i++)
            {
                string soundFilePath = soundFilesDirPath + adviceParts[i] + ".mp3";
                try
                {
                    System.IO.Stream @is = new System.IO.FileStream(soundFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    int availableBytes = 0;// @is.Available();
                    byte[] tmp = new byte[availableBytes];
                    @is.Read(tmp, 0, availableBytes);
                    if (stream != null)
                    {
                        stream.Write(tmp);
                    }
                    @is.Close();
                    validTokensFound = true;
                }
                catch (IOException ioe)
                {
                    Console.WriteLine(ioe.ToString());
                    Console.Write(ioe.StackTrace);
                }
            }

            if (validTokensFound)
            {
                // valid tokens were found - set busy state until finishing to play
                // advice
                isBusy = true;
            }
            else
            {
                // valid tokens not found - return without playing anything
                return;
            }

            writeFile(stream.ToByteArray(), tempAdviceFile);
            playFile(tempAdviceFile);
        }

        public virtual void reset()
        {
            Log.Warn(TAG, "Entering reset");
            if (player != null)
            {
                try
                {
                    player.Reset();
                    deleteTempFile();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.Write(ex.StackTrace);
                }
            }
            isBusy = false;
        }

        /// <summary>
        /// Deletes the temporary file stored at "tempAdviceFile" path
        /// </summary>
        private void deleteTempFile()
        {
            File fc = new File(tempAdviceFile);
            if (fc.Exists())
            {
                fc.Delete();
            }
        }

        /// <summary>
        /// Stops playing the current advice
        /// </summary>
        public virtual void stop()
        {
            isBusy = false;
            player.Stop();
        }

        /// <summary>
        /// Writes "data" to the "filePath" path on the disk </summary>
        /// <param name="data"> </param>
        /// <param name="filePath"> </param>
        private void writeFile(byte[] data, string filePath)
        {
            System.IO.Stream @out = null;
            try
            {
                @out = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                @out.Write(data, 0, data.Length);
                try
                {
                    @out.Flush();
                    @out.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                }

            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe.ToString());
                Console.Write(ioe.StackTrace);
            }
        }

        /// <summary>
        /// Plays an .mp3 file which should be found at filePath </summary>
        /// <param name="filePath"> </param>
        private void playFile(string filePath)
        {
            try
            {
                player.Reset();
                File file = new File(filePath);
                System.IO.FileStream fileInputStream = null;//new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                FileDescriptor fileDescriptor = null;// fileInputStream.FD;
                try
                {
                    player.SetDataSource(fileDescriptor);
                }
                catch (System.InvalidOperationException)
                {
                    player.Reset();
                    player.SetDataSource(fileDescriptor);
                }
                fileInputStream.Close();

                player.Prepare();
                player.Start();
            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe.ToString());
                Console.Write(ioe.StackTrace);
            }
        }

        public void OnCompletion(MediaPlayer mp)
        {
            reset();
            if (nextAdvice != null)
            {
                string[] adviceToPlay = nextAdvice;
                nextAdvice = null;
                playAdvice(adviceToPlay, nextAdvicePriority);
                return;
            }
        }

        public bool OnError(MediaPlayer mp, MediaError what, int extra)
        {
            return true; //error was handled
        }
    }
}