﻿namespace Skobbler.SDKDemo.Activity
{
	public class SettingsActivity : PreferenceActivity
	{


		protected internal override void onCreate(Bundle savedInstanceState)
		{
			base.onCreate(savedInstanceState);
			addPreferencesFromResource(R.xml.settings);
			ListPreference listPreference = (ListPreference) findPreference(PreferenceTypes.K_ROUTE_TYPE);
			listPreference.OnPreferenceChangeListener = new OnPreferenceChangeListenerAnonymousInnerClassHelper(this, listPreference);
			ListPreference listDistanceFormat = (ListPreference) findPreference(PreferenceTypes.K_DISTANCE_UNIT);
			if (listDistanceFormat.Value == null)
			{
				listDistanceFormat.ValueIndex = 0;
			}
			listDistanceFormat.OnPreferenceChangeListener = new OnPreferenceChangeListenerAnonymousInnerClassHelper2(this, listDistanceFormat);

			ListPreference listNavigationType = (ListPreference) findPreference(PreferenceTypes.K_NAVIGATION_TYPE);
			if (listNavigationType.Value == null)
			{
				listNavigationType.ValueIndex = 1;
			}
			listNavigationType.OnPreferenceChangeListener = new OnPreferenceChangeListenerAnonymousInnerClassHelper3(this, listNavigationType);

			ListPreference listSpeedWarningsInTown = (ListPreference) findPreference(PreferenceTypes.K_IN_TOWN_SPEED_WARNING);
			if (listDistanceFormat.Summary.Equals("Kilometers/Meters"))
			{
				listSpeedWarningsInTown.Entries = new string[]{"5km/h", "10km/h", "15km/h", "20km/h", "25km/h"};
				listSpeedWarningsInTown.EntryValues = new string[]{"0", "1", "2", "3", "4"};
			}
			else
			{
				listSpeedWarningsInTown.Entries = new string[]{"5mi/h", "10mi/h", "15mi/h", "20mi/h", "25mi/h"};
				listSpeedWarningsInTown.EntryValues = new string[]{"0", "1", "2", "3", "4"};
			}
			if (listSpeedWarningsInTown.Value == null)
			{
				listSpeedWarningsInTown.ValueIndex = 3;
			}
			listSpeedWarningsInTown.OnPreferenceChangeListener = new OnPreferenceChangeListenerAnonymousInnerClassHelper4(this, listSpeedWarningsInTown);

			ListPreference listSpeedWarningsOutTown = (ListPreference) findPreference(PreferenceTypes.K_OUT_TOWN_SPEED_WARNING);
			if (listDistanceFormat.Summary.Equals("Kilometers/Meters"))
			{
				listSpeedWarningsOutTown.Entries = new string[]{"5km/h", "10km/h", "15km/h", "20km/h", "25km/h"};
				listSpeedWarningsOutTown.EntryValues = new string[]{"0", "1", "2", "3", "4"};
			}
			else
			{
				listSpeedWarningsOutTown.Entries = new string[]{"5mi/h", "10mi/h", "15mi/h", "20mi/h", "25mi/h"};
				listSpeedWarningsOutTown.EntryValues = new string[]{"0", "1", "2", "3", "4"};
			}
			if (listSpeedWarningsOutTown.Value == null)
			{
				listSpeedWarningsOutTown.ValueIndex = 3;
			}
			listSpeedWarningsOutTown.OnPreferenceChangeListener = new OnPreferenceChangeListenerAnonymousInnerClassHelper5(this, listSpeedWarningsOutTown);

			CheckBoxPreference checkBoxDayNight = (CheckBoxPreference) findPreference(PreferenceTypes.K_AUTO_DAY_NIGHT);
			checkBoxDayNight.OnPreferenceChangeListener = new OnPreferenceChangeListenerAnonymousInnerClassHelper6(this);

			CheckBoxPreference checkBoxTollRoads = (CheckBoxPreference) findPreference(PreferenceTypes.K_AVOID_TOLL_ROADS);
			checkBoxTollRoads.OnPreferenceChangeListener = new OnPreferenceChangeListenerAnonymousInnerClassHelper7(this);

			CheckBoxPreference checkBoxFerries = (CheckBoxPreference) findPreference(PreferenceTypes.K_AVOID_FERRIES);
			checkBoxFerries.OnPreferenceChangeListener = new OnPreferenceChangeListenerAnonymousInnerClassHelper8(this);

			CheckBoxPreference checkBoxHighways = (CheckBoxPreference) findPreference(PreferenceTypes.K_AVOID_HIGHWAYS);
			checkBoxHighways.OnPreferenceChangeListener = new OnPreferenceChangeListenerAnonymousInnerClassHelper9(this);

			CheckBoxPreference checkBoxFreeDrive = (CheckBoxPreference) findPreference(PreferenceTypes.K_FREE_DRIVE);
			checkBoxFreeDrive.OnPreferenceChangeListener = new OnPreferenceChangeListenerAnonymousInnerClassHelper10(this);

		}

		private class OnPreferenceChangeListenerAnonymousInnerClassHelper : Preference.OnPreferenceChangeListener
		{
			private readonly SettingsActivity outerInstance;

			private ListPreference listPreference;

			public OnPreferenceChangeListenerAnonymousInnerClassHelper(SettingsActivity outerInstance, ListPreference listPreference)
			{
				this.outerInstance = outerInstance;
				this.listPreference = listPreference;
			}


			public override bool onPreferenceChange(Preference preference, object newValue)
			{
				// Set the value as the new value
				listPreference.Value = newValue.ToString();
				// Get the entry which corresponds to the current value and set as summary
				preference.Summary = listPreference.Entry;
				if (preference.Summary.Equals("Car shortest"))
				{
					PreferenceScreen.findPreference("pref_routes_number").Summary = "1";
					PreferenceScreen.findPreference("pref_routes_number").Enabled = false;
				}
				else if (preference.Summary.Equals("Bicycle shortest"))
				{
					PreferenceScreen.findPreference("pref_routes_number").Summary = "1";
					PreferenceScreen.findPreference("pref_routes_number").Enabled = false;
				}
				return false;
			}
		}

		private class OnPreferenceChangeListenerAnonymousInnerClassHelper2 : Preference.OnPreferenceChangeListener
		{
			private readonly SettingsActivity outerInstance;

			private ListPreference listDistanceFormat;

			public OnPreferenceChangeListenerAnonymousInnerClassHelper2(SettingsActivity outerInstance, ListPreference listDistanceFormat)
			{
				this.outerInstance = outerInstance;
				this.listDistanceFormat = listDistanceFormat;
			}


			public override bool onPreferenceChange(Preference preference, object newValue)
			{
				// Set the value as the new value
				listDistanceFormat.Value = newValue.ToString();
				// Get the entry which corresponds to the current value and set as summary
				preference.Summary = listDistanceFormat.Entry;
				if (preference.Summary.Equals("Miles/Feet") || preference.Summary.Equals("Miles/Yards"))
				{

					ListPreference listSpeedWarningsInTown = (ListPreference) findPreference("pref_speed_warnings_in_town");
					listSpeedWarningsInTown.Entries = new string[]{"5mi/h", "10mi/h", "15mi/h", "20mi/h", "25mi/h"};
					listSpeedWarningsInTown.EntryValues = new string[]{"0", "1", "2", "3", "4"};
					ListPreference listSpeedWarningsOutTown = (ListPreference) findPreference("pref_speed_warnings_out_town");
					listSpeedWarningsOutTown.Entries = new string[]{"5mi/h", "10mi/h", "15mi/h", "20mi/h", "25mi/h"};
					listSpeedWarningsOutTown.EntryValues = new string[]{"0", "1", "2", "3", "4"};
				}
				else if (preference.Summary.Equals("Kilometers/Meters"))
				{
					ListPreference listSpeedWarningsInTown = (ListPreference) findPreference("pref_speed_warnings_in_town");
					listSpeedWarningsInTown.Entries = new string[]{"5km/h", "10km/h", "15km/h", "20km/h", "25km/h"};
					listSpeedWarningsInTown.EntryValues = new string[]{"0", "1", "2", "3", "4"};
					ListPreference listSpeedWarningsOutTown = (ListPreference) findPreference("pref_speed_warnings_out_town");
					listSpeedWarningsOutTown.Entries = new string[]{"5km/h", "10km/h", "15km/h", "20km/h", "25km/h"};
					listSpeedWarningsOutTown.EntryValues = new string[]{"0", "1", "2", "3", "4"};
				}
				return false;
			}
		}

		private class OnPreferenceChangeListenerAnonymousInnerClassHelper3 : Preference.OnPreferenceChangeListener
		{
			private readonly SettingsActivity outerInstance;

			private ListPreference listNavigationType;

			public OnPreferenceChangeListenerAnonymousInnerClassHelper3(SettingsActivity outerInstance, ListPreference listNavigationType)
			{
				this.outerInstance = outerInstance;
				this.listNavigationType = listNavigationType;
			}


			public override bool onPreferenceChange(Preference preference, object newValue)
			{
				// Set the value as the new value
				listNavigationType.Value = newValue.ToString();
				// Get the entry which corresponds to the current value and set as summary
				preference.Summary = listNavigationType.Entry;
				return false;
			}
		}

		private class OnPreferenceChangeListenerAnonymousInnerClassHelper4 : Preference.OnPreferenceChangeListener
		{
			private readonly SettingsActivity outerInstance;

			private ListPreference listSpeedWarningsInTown;

			public OnPreferenceChangeListenerAnonymousInnerClassHelper4(SettingsActivity outerInstance, ListPreference listSpeedWarningsInTown)
			{
				this.outerInstance = outerInstance;
				this.listSpeedWarningsInTown = listSpeedWarningsInTown;
			}


			public override bool onPreferenceChange(Preference preference, object newValue)
			{
				// Set the value as the new value
				listSpeedWarningsInTown.Value = newValue.ToString();
				// Get the entry which corresponds to the current value and set as summary
				preference.Summary = listSpeedWarningsInTown.Entry;
				return false;
			}
		}

		private class OnPreferenceChangeListenerAnonymousInnerClassHelper5 : Preference.OnPreferenceChangeListener
		{
			private readonly SettingsActivity outerInstance;

			private ListPreference listSpeedWarningsOutTown;

			public OnPreferenceChangeListenerAnonymousInnerClassHelper5(SettingsActivity outerInstance, ListPreference listSpeedWarningsOutTown)
			{
				this.outerInstance = outerInstance;
				this.listSpeedWarningsOutTown = listSpeedWarningsOutTown;
			}


			public override bool onPreferenceChange(Preference preference, object newValue)
			{
				// Set the value as the new value
				listSpeedWarningsOutTown.Value = newValue.ToString();
				// Get the entry which corresponds to the current value and set as summary
				preference.Summary = listSpeedWarningsOutTown.Entry;
				return false;
			}
		}

		private class OnPreferenceChangeListenerAnonymousInnerClassHelper6 : Preference.OnPreferenceChangeListener
		{
			private readonly SettingsActivity outerInstance;

			public OnPreferenceChangeListenerAnonymousInnerClassHelper6(SettingsActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}


			public override bool onPreferenceChange(Preference preference, object newValue)
			{
				Log.d("MyApp", "Pref " + preference.Key + " changed to " + newValue.ToString());
				return true;
			}
		}

		private class OnPreferenceChangeListenerAnonymousInnerClassHelper7 : Preference.OnPreferenceChangeListener
		{
			private readonly SettingsActivity outerInstance;

			public OnPreferenceChangeListenerAnonymousInnerClassHelper7(SettingsActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}


			public override bool onPreferenceChange(Preference preference, object newValue)
			{
				Log.d("MyApp", "Pref " + preference.Key + " changed to " + newValue.ToString());
				return true;
			}
		}

		private class OnPreferenceChangeListenerAnonymousInnerClassHelper8 : Preference.OnPreferenceChangeListener
		{
			private readonly SettingsActivity outerInstance;

			public OnPreferenceChangeListenerAnonymousInnerClassHelper8(SettingsActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}


			public override bool onPreferenceChange(Preference preference, object newValue)
			{
				Log.d("MyApp", "Pref " + preference.Key + " changed to " + newValue.ToString());
				return true;
			}
		}

		private class OnPreferenceChangeListenerAnonymousInnerClassHelper9 : Preference.OnPreferenceChangeListener
		{
			private readonly SettingsActivity outerInstance;

			public OnPreferenceChangeListenerAnonymousInnerClassHelper9(SettingsActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}


			public override bool onPreferenceChange(Preference preference, object newValue)
			{
				Log.d("MyApp", "Pref " + preference.Key + " changed to " + newValue.ToString());
				return true;
			}
		}

		private class OnPreferenceChangeListenerAnonymousInnerClassHelper10 : Preference.OnPreferenceChangeListener
		{
			private readonly SettingsActivity outerInstance;

			public OnPreferenceChangeListenerAnonymousInnerClassHelper10(SettingsActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}


			public override bool onPreferenceChange(Preference preference, object newValue)
			{
				Log.d("MyApp", "Pref " + preference.Key + " changed to " + newValue.ToString());
				return true;
			}
		}

	}

}