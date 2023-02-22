using System.Globalization;

namespace Timotheus
{
    public class Localization
    {
        private static Culture CurrentCulture = Culture.Default;
        public static CultureInfo LocalizationCulture
        {
            get
            {
                return CultureInfo.GetCultureInfo("da-DK");
            }
            set
            {
                if (value.Name == "da-DK")
                    CurrentCulture = Culture.daDK;
            }
        }

        private enum Culture
        {
            daDK,
            Default
        }

        /// <summary>
        ///   Looks up a localized string similar to Add person.
        /// </summary>
        public static string AddConsentForm
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Tilføj person";
                    default:
                        return "Add person";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Add.
        /// </summary>
        public static string AddConsentForm_AddButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Tilføj";
                    default:
                        return "Add";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Edit.
        /// </summary>
        public static string AddConsentForm_EditButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ændr";
                    default:
                        return "Edit";
                }
            }
        }

		/// <summary>
		///   Looks up a localized string similar to Edit.
		/// </summary>
		public static string AddConsentForm_DeleteButton
		{
			get
			{
				switch (CurrentCulture)
				{
					case Culture.daDK:
						return "Slet";
					default:
						return "Delete";
				}
			}
		}

		/// <summary>
		///   Looks up a localized string similar to Cancel.
		/// </summary>
		public static string AddConsentForm_CancelButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Annullér";
                    default:
                        return "Cancel";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Comment.
        /// </summary>
        public static string AddConsentForm_Comment
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Kommentar";
                    default:
                        return "Comment";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Date.
        /// </summary>
        public static string AddConsentForm_Date
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Dato";
                    default:
                        return "Date";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The name cannot be empty.
        /// </summary>
        public static string AddConsentForm_EmptyName
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Navnet kan ikke være tomt";
                    default:
                        return "The name cannot be empty";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to An empty version field is interpreted as no consent. Are you sure you want to add the person?.
        /// </summary>
        public static string AddConsentForm_EmptyVersion
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Et tomt versionsfelt tolkes som ingen samtykke. Er du sikker på at du stadig vil tilføje personen?";
                    default:
                        return "An empty version field is interpreted as no consent. Are you sure you want to add the person?";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Name.
        /// </summary>
        public static string AddConsentForm_Name
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Navn";
                    default:
                        return "Name";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Version.
        /// </summary>
        public static string AddConsentForm_Version
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Version";
                    default:
                        return "Version";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Add event.
        /// </summary>
        public static string AddEvent
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Tilføj begivenhed";
                    default:
                        return "Add event";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Add.
        /// </summary>
        public static string AddEvent_AddButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Tilføj";
                    default:
                        return "Add";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to All day event.
        /// </summary>
        public static string AddEvent_AllDayBox
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Heldagsbegivenhed";
                    default:
                        return "All day event";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        public static string AddEvent_CancelButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Annullér";
                    default:
                        return "Cancel";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Descrip..
        /// </summary>
        public static string AddEvent_DescriptionLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Beskriv.";
                    default:
                        return "Descrip.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Edit event.
        /// </summary>
        public static string AddEvent_Edit
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ændr begivenhed";
                    default:
                        return "Edit event";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Edit.
        /// </summary>
        public static string AddEvent_EditButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ændr";
                    default:
                        return "Edit";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to End*.
        /// </summary>
        public static string AddEvent_EndLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Slut*";
                    default:
                        return "End*";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Location.
        /// </summary>
        public static string AddEvent_LocationLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Adresse";
                    default:
                        return "Location";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Name*.
        /// </summary>
        public static string AddEvent_NameLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Navn*";
                    default:
                        return "Name*";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Start*.
        /// </summary>
        public static string AddEvent_StartLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Start*";
                    default:
                        return "Start*";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Add Event.
        /// </summary>
        public static string Calendar_AddButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Tilføj";
                    default:
                        return "Add Event";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Add an event to the calendar.
        /// </summary>
        public static string Calendar_AddEvent_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Tilføj en begivenhed til kalenderen";
                    default:
                        return "Add an event to the calendar";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to All.
        /// </summary>
        public static string Calendar_All
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Alle";
                    default:
                        return "All";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to All.
        /// </summary>
        public static string Calendar_AllButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Alle";
                    default:
                        return "All";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Go back in time.
        /// </summary>
        public static string Calendar_BackTime_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gå tilbage i tiden";
                    default:
                        return "Go back in time";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Are you sure you want to delete the event &quot;#&quot;?.
        /// </summary>
        public static string Calendar_DeleteEvent
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Er du sikker på at du vil slette begivenheden \"#\"?";
                    default:
                        return "Are you sure you want to delete the event \"#\"?";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Delete event.
        /// </summary>
        public static string Calendar_DeleteEvent_Tooltip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Slet begivenhed";
                    default:
                        return "Delete event";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Description.
        /// </summary>
        public static string Calendar_DescriptionColumn
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "  Beskrivelse";
                    default:
                        return "  Description";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Edit event.
        /// </summary>
        public static string Calendar_EditEvent_Tooltip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ændr begivenhed";
                    default:
                        return "Edit event";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to End.
        /// </summary>
        public static string Calendar_EndColumn
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Slut";
                    default:
                        return "End";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Exports the calendar as PDF, and only shows events in the current period..
        /// </summary>
        public static string Calendar_Export_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Eksportér kalenderen som PDF med begivenheder i den valgte periode";
                    default:
                        return "Exports the calendar as PDF, and only shows events in the current period.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Export.
        /// </summary>
        public static string Calendar_ExportButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Eksportér";
                    default:
                        return "Export";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Fall.
        /// </summary>
        public static string Calendar_Fall
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Efteråret";
                    default:
                        return "Fall";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Go forward in time.
        /// </summary>
        public static string Calendar_ForwardTime_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gå fremad i tiden";
                    default:
                        return "Go forward in time";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Half-year.
        /// </summary>
        public static string Calendar_HalfYearButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Halvår";
                    default:
                        return "Half-year";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Location.
        /// </summary>
        public static string Calendar_LocationColumn
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Adresse";
                    default:
                        return "Location";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Month.
        /// </summary>
        public static string Calendar_MonthButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Måned";
                    default:
                        return "Month";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Name.
        /// </summary>
        public static string Calendar_NameColumn
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Navn";
                    default:
                        return "Name";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Open a calendar.
        /// </summary>
        public static string Calendar_Open_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Åben en kalender";
                    default:
                        return "Open a calendar";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Calendar.
        /// </summary>
        public static string Calendar_Page
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Kalender";
                    default:
                        return "Calendar";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The current period. The calendar will only show events in this period..
        /// </summary>
        public static string Calendar_PeriodBox_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Den nuværende periode. Der vises kun begivenheder der er i denne periode.";
                    default:
                        return "The current period. The calendar will only show events in this period.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The current type of period..
        /// </summary>
        public static string Calendar_PeriodType_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Den nuværende slags periode.";
                    default:
                        return "The current type of period.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Save the calendar as a .ics file. This does NOT synchronize the calendar..
        /// </summary>
        public static string Calendar_Save_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gem kalenderen som en .ics fil. Dette synkronisere IKKE kalenderen.";
                    default:
                        return "Save the calendar as a .ics file. This does NOT synchronize the calendar.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Spring.
        /// </summary>
        public static string Calendar_Spring
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Foråret";
                    default:
                        return "Spring";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Start.
        /// </summary>
        public static string Calendar_StartColumn
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Start";
                    default:
                        return "Start";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Synchronize the calendar with a remote calendar.
        /// </summary>
        public static string Calendar_Sync_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Synkroniser kalenderen med en anden";
                    default:
                        return "Synchronize the calendar with a remote calendar";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Synchronize.
        /// </summary>
        public static string Calendar_SyncButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Synkronisér";
                    default:
                        return "Synchronize";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Year.
        /// </summary>
        public static string Calendar_YearButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "År";
                    default:
                        return "Year";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        public static string Cancel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Annullér";
                    default:
                        return "Cancel";
                }
            }
        }

		/// <summary>
		///   Looks up a localized string similar to Confirmation.
		/// </summary>
		public static string Confirmation
		{
			get
			{
				switch (CurrentCulture)
				{
					case Culture.daDK:
						return "Bekræftelse";
					default:
						return "Confirmation";
				}
			}
		}

		/// <summary>
		///   Looks up a localized string similar to Confirmation.
		/// </summary>
		public static string Confirmation_SaveConsentForms
		{
			get
			{
				switch (CurrentCulture)
				{
					case Culture.daDK:
						return "Samtykkeerklæringerne er blevet gemt.";
					default:
						return "The list of people have been saved.";
				}
			}
		}

		/// <summary>
		///   Looks up a localized string similar to Edit person.
		/// </summary>
		public static string EditPersonDialog
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ændr person";
                    default:
                        return "Edit person";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Edit.
        /// </summary>
        public static string EditKeyDialog
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Redigér";
                    default:
                        return "Edit";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Add standard keys.
        /// </summary>
        public static string EditKeyDialog_AddStdButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Tilføj standard nøgler";
                    default:
                        return "Add standard keys";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        public static string EditKeyDialog_CancelButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Annullér";
                    default:
                        return "Cancel";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Ok.
        /// </summary>
        public static string EditKeyDialog_OKButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ok";
                    default:
                        return "Ok";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The program is already running. If you can&apos;t see it, check the system tray or task manager.
        /// </summary>
        public static string Exception_AlreadyRunning
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Programmet kører allerede, hvis du ikke kan se det, tjek systembakken eller joblisten";
                    default:
                        return "The program is already running. If you can't see it, check the system tray or task manager";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to You can&apos;t go further back. This is the root directory..
        /// </summary>
        public static string Exception_CantGoUpDirectory
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Du kan ikke gå længere tilbage.";
                    default:
                        return "You can't go further back. This is the root directory.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Copy error
        /// </summary>
        public static string Exception_ConversionError
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Det lykkedes ikke programmet at oversætte filsti.";
                    default:
                        return "The program failed to translate filepath.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Copy error
        /// </summary>
        public static string Exception_CopyError
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Kopiér fejl";
                    default:
                        return "Copy error";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Directory with that name already exists
        /// </summary>
        public static string Exception_DirectoryAlreadyExists
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Mappe med det navn findes allerede";
                    default:
                        return "Directory with that name already exists";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Address cannot be empty.
        /// </summary>
        public static string Exception_EmptyAddress
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Adresse må ikke være tom";
                    default:
                        return "Address cannot be empty";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to ICS field cannot be empty.
        /// </summary>
        public static string Exception_EmptyICS
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "URL felt må ikke være tom";
                    default:
                        return "ICS field cannot be empty";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Name cannot be empty.
        /// </summary>
        public static string Exception_EmptyName
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Navn må ikke være tom";
                    default:
                        return "Name cannot be empty";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to End date cannot be before the start date..
        /// </summary>
        public static string Exception_EndBeforeStart
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Slut dato kan ikke være før start.";
                    default:
                        return "End date cannot be before the start date.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Copy File with that name already exists
        /// </summary>
        public static string Exception_FileAlreadyExists
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Fil med det navn findes allerede";
                    default:
                        return "File with that name already exists";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The local folder couldn&apos;t be found. Do you want to find a new folder for syncing?.
        /// </summary>
        public static string Exception_FolderNotFound
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Mappen til fildeling kunne ikke findes. Vil du vælge en ny mappe?";
                    default:
                        return "The local folder couldn't be found. Do you want to find a new folder for syncing?";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Image couldn&apos;t be found. Choose another..
        /// </summary>
        public static string Exception_ImageNotFound
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Billede kunne ikke findes. Vælg et andet.";
                    default:
                        return "Image couldn't be found. Choose another.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Calendar isn&apos;t formatted correctly.
        /// </summary>
        public static string Exception_InvalidCalendar
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Kalender er ikke formateret korrekt";
                    default:
                        return "Calendar isn't formatted correctly";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to This was not a valid event..
        /// </summary>
        public static string Exception_InvalidEvent
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Begivenheden var ikke brugbar.";
                    default:
                        return "This was not a valid event.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Invalid input.
        /// </summary>
        public static string Exception_InvalidInput
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ugyldig input";
                    default:
                        return "Invalid input";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to This is an invalid password. Try another....
        /// </summary>
        public static string Exception_InvalidPassword
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Dette er ikke en brugbar password. Prøv en anden...";
                    default:
                        return "This is an invalid password. Try another...";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Couldn&apos;t load file.
        /// </summary>
        public static string Exception_LoadFailed
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Kunne ikke læse fil";
                    default:
                        return "Couldn't load file";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Message.
        /// </summary>
        public static string Exception_Message
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Besked";
                    default:
                        return "Message";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Error.
        /// </summary>
        public static string Exception_Name
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Fejl";
                    default:
                        return "Error";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to A connection to the internet could not be established. Check your internet conneciton or firewall..
        /// </summary>
        public static string Exception_NoInternet
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Der kunne ikke skabes en forbindelse til internettet. Tjek din internetforbindelse eller firewall.";
                    default:
                        return "A connection to the internet could not be established. Check your internet conneciton or firewall.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Key file could not be loaded.
        /// </summary>
        public static string Exception_NoKeys
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Nøglefil kunne ikke læses";
                    default:
                        return "Key file could not be loaded";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The synchronization interval must be more than zero.
        /// </summary>
        public static string Exception_SyncInterval_MoreThanOne
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Synkroniserings intervallet skal være over nul.";
                    default:
                        return "The synchronization interval must be more than zero.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to File is only on the server. Synchronize first..
        /// </summary>
        public static string Exception_OnlineFile
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Filen er kun på serveren. Synkronisér for at downloade filen.";
                    default:
                        return "File is only on the server. Synchronize first.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Archive could not be found. Try choosing another folder in &quot;Export&quot;..
        /// </summary>
        public static string Exception_PDFArchiveNotFound
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Arkivet kunne ikke findes. Prøv at vælge en anden mappe i \"Eksport\".";
                    default:
                        return "Archive could not be found. Try choosing another folder in \"Export\".";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The export path cannot be empty..
        /// </summary>
        public static string Exception_PDFEmptyExport
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Eksport feltet kan ikke være tomt.";
                    default:
                        return "The export path cannot be empty.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to File saved!.
        /// </summary>
        public static string Exception_SaveSuccessful
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Fil gemt!";
                    default:
                        return "File saved!";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Couldn&apos;t save file.
        /// </summary>
        public static string Exception_Saving
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Kunne ikke gemme fil";
                    default:
                        return "Couldn't save file";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The file is not in the directory.
        /// </summary>
        public static string Exception_SFTPInvalidPath
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Filen er ikke i mappen";
                    default:
                        return "The file is not in the directory";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Couldn&apos;t sync the calendar.
        /// </summary>
        public static string Exception_Sync
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Kunne ikke synkronisere";
                    default:
                        return "Couldn't sync the calendar";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Couldn&apos;t sync the files.
        /// </summary>
        public static string Exception_SyncSFTP
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Kunne ikke synkronisere filerne";
                    default:
                        return "Couldn't sync the files";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Are you sure you want to quit? There is unsaved progress..
        /// </summary>
        public static string Exception_UnsavedProgress
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Er du sikker på du vil lukke? Der er ændringer som ikke er blevet gemt.";
                    default:
                        return "Are you sure you want to quit? There is unsaved progress.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Warning.
        /// </summary>
        public static string Exception_Warning
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Advarsel";
                    default:
                        return "Warning";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Wrong password.
        /// </summary>
        public static string Exception_WrongPassword
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Forkert adgangskode";
                    default:
                        return "Wrong password";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Timotheus is a program which seeks to help assocations with their calendar, file sharing, and hopefully a lot more in the future!.
        /// </summary>
        public static string FirstTime_1P
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Timotheus er et program som skal hjælpe foreninger med deres kalender, fildeling og forhåbentlig mange flere ting i fremtiden!";
                    default:
                        return "Timotheus is a program which seeks to help assocations with their calendar, file sharing, and hopefully a lot more in the future!";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The program uses a key file (.tkey) to connect all these services by storing usernames, passwords etc. in a encrypted file..
        /// </summary>
        public static string FirstTime_2P
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Dette program bruger nøglefiler (.tkey) til at forbinde alle disse services ved at opbevare brugernavne, adgangskoder osv. i en krypteret fil.";
                    default:
                        return "The program uses a key file (.tkey) to connect all these services by storing usernames, passwords etc. in a encrypted file.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to How do you want to start?.
        /// </summary>
        public static string FirstTime_3P
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Hvordan vil du starte?";
                    default:
                        return "How do you want to start?";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Welcome.
        /// </summary>
        public static string FirstTime_Welcome
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Velkommen";
                    default:
                        return "Welcome";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to With key.
        /// </summary>
        public static string FirstTime_With
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Med nøgle";
                    default:
                        return "With key";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Without key.
        /// </summary>
        public static string FirstTime_Without
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Uden nøgle";
                    default:
                        return "Without key";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Help.
        /// </summary>
        public static string Help
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Hjælp";
                    default:
                        return "Help";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Close.
        /// </summary>
        public static string Help_Close
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Luk";
                    default:
                        return "Close";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Contributors:.
        /// </summary>
        public static string Help_ContributorsLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Forfattere:";
                    default:
                        return "Contributors:";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Email:.
        /// </summary>
        public static string Help_EmailLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Email:";
                    default:
                        return "Email:";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to License:.
        /// </summary>
        public static string Help_LicenseLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Licens:";
                    default:
                        return "License:";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Source:.
        /// </summary>
        public static string Help_SourceLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Kilde:";
                    default:
                        return "Source:";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Version:.
        /// </summary>
        public static string Help_VersionLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Version:";
                    default:
                        return "Version:";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to New key.
        /// </summary>
        public static string InsertKey_Args
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ny nøgle";
                    default:
                        return "New key";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Do you want to open with the key '#'? Unsaved changes made with the current key, will not be saved..
        /// </summary>
        public static string InsertKey_ArgsMessage
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Vil du åbne med nøglen '#'? Det som ikke er gemt med den nuværende nøgle, vil ikke blive gemt.";
                    default:
                        return "Do you want to open with the key '#'? Unsaved changes made with the current key, will not be saved.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Changes detected.
        /// </summary>
        public static string InsertKey_ChangeDetected
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ændringer";
                    default:
                        return "Changes detected";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Inserting key.
        /// </summary>
        public static string InsertKey_Dialog
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Indlæser nøgle";
                    default:
                        return "Inserting key";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Do you want to save the changes made to the key?.
        /// </summary>
        public static string InsertKey_DoYouWantToSave
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Vil du gemme ændringerne til nøglen?";
                    default:
                        return "Do you want to save the changes made to the key?";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Opening calendar.
        /// </summary>
        public static string InsertKey_LoadCalendar
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Åbner kalender";
                    default:
                        return "Opening calendar";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Connecting to remote directory.
        /// </summary>
        public static string InsertKey_LoadFiles
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Forbinder til filer";
                    default:
                        return "Connecting to remote directory";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Loading people.
        /// </summary>
        public static string InsertKey_LoadPeople
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Indlæser personer";
                    default:
                        return "Loading people";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to OK.
        /// </summary>
        public static string OK
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "OK";
                    default:
                        return "OK";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Open.
        /// </summary>
        public static string OpenCalendar
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Åben";
                    default:
                        return "Open";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Browse.
        /// </summary>
        public static string OpenCalendar_BrowseButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gennemse";
                    default:
                        return "Browse";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Remote.
        /// </summary>
        public static string OpenCalendar_CalDAVButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Online";
                    default:
                        return "Remote";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        public static string OpenCalendar_CancelButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Annullér";
                    default:
                        return "Cancel";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Local.
        /// </summary>
        public static string OpenCalendar_ICSButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Lokal";
                    default:
                        return "Local";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Path.
        /// </summary>
        public static string OpenCalendar_ICSLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Fil";
                    default:
                        return "Path";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Open.
        /// </summary>
        public static string OpenCalendar_OpenButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Åben";
                    default:
                        return "Open";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Password.
        /// </summary>
        public static string OpenCalendar_PasswordLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Adgangskode";
                    default:
                        return "Password";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to URL.
        /// </summary>
        public static string OpenCalendar_URLLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "URL";
                    default:
                        return "URL";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Username.
        /// </summary>
        public static string OpenCalendar_UsernameLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Brugernavn";
                    default:
                        return "Username";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Password required.
        /// </summary>
        public static string PasswordDialog
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Adgangskode nødvendig";
                    default:
                        return "Password required";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        public static string PasswordDialog_CancelButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Annullér";
                    default:
                        return "Cancel";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Password.
        /// </summary>
        public static string PasswordDialog_Label
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Adgangskode";
                    default:
                        return "Password";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Ok.
        /// </summary>
        public static string PasswordDialog_OKButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ok";
                    default:
                        return "Ok";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Save password.
        /// </summary>
        public static string PasswordDialog_SaveBox
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gem adgangskode";
                    default:
                        return "Save password";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Export PDF.
        /// </summary>
        public static string PDF
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Eksportér PDF";
                    default:
                        return "Export PDF";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Activity.
        /// </summary>
        public static string PDF_Activity
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Aktivitet";
                    default:
                        return "Activity";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Archive.
        /// </summary>
        public static string PDF_ArchivePath
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Arkiv";
                    default:
                        return "Archive";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Backpage.
        /// </summary>
        public static string PDF_Backpage
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Bagside";
                    default:
                        return "Backpage";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Browse.
        /// </summary>
        public static string PDF_Browse
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gennemse";
                    default:
                        return "Browse";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        public static string PDF_Cancel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Annullér";
                    default:
                        return "Cancel";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Coffee.
        /// </summary>
        public static string PDF_Coffee
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Kaffehold";
                    default:
                        return "Coffee";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Comment.
        /// </summary>
        public static string PDF_Comment
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Kommentar";
                    default:
                        return "Comment";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Date.
        /// </summary>
        public static string PDF_Date
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Dato";
                    default:
                        return "Date";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Export.
        /// </summary>
        public static string PDF_ExportPath
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Eksport";
                    default:
                        return "Export";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Footer.
        /// </summary>
        public static string PDF_Footer
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Sidefod";
                    default:
                        return "Footer";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Leader.
        /// </summary>
        public static string PDF_Leader
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Mødeleder";
                    default:
                        return "Leader";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Logo.
        /// </summary>
        public static string PDF_Logo
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Logo";
                    default:
                        return "Logo";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Musician.
        /// </summary>
        public static string PDF_Musician
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Musiker";
                    default:
                        return "Musician";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Export.
        /// </summary>
        public static string PDF_Ok
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Eksportér";
                    default:
                        return "Export";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Schedule for.
        /// </summary>
        public static string PDF_Schedule
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Program for";
                    default:
                        return "Schedule for";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Start.
        /// </summary>
        public static string PDF_Start
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Start";
                    default:
                        return "Start";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Subtitle.
        /// </summary>
        public static string PDF_Subtitle
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Undertitel";
                    default:
                        return "Subtitle";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Title.
        /// </summary>
        public static string PDF_Title
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Titel";
                    default:
                        return "Title";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Book.
        /// </summary>
        public static string PDF_Type_Book
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Bog";
                    default:
                        return "Book";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Table.
        /// </summary>
        public static string PDF_Type_Table
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Tabel";
                    default:
                        return "Table";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Welcome to.
        /// </summary>
        public static string PDF_Welcome
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Velkommen i";
                    default:
                        return "Welcome to";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Active.
        /// </summary>
        public static string People_Active
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Aktiv";
                    default:
                        return "Active";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Make the person active/inactive.
        /// </summary>
        public static string People_Active_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gør samtykkeerklæringen aktiv/inaktiv";
                    default:
                        return "Make the person active/inactive";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Add person.
        /// </summary>
        public static string People_Add
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Tilføj person";
                    default:
                        return "Add person";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Add person.
        /// </summary>
        public static string People_Add_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Tilføj samtykkeerklæring";
                    default:
                        return "Add person";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Comment.
        /// </summary>
        public static string People_Comment
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Kommentar";
                    default:
                        return "Comment";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Date.
        /// </summary>
        public static string People_ConsentDate
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Dato";
                    default:
                        return "Date";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Version.
        /// </summary>
        public static string People_ConsentVersion
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Version";
                    default:
                        return "Version";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Are you sure you want to delete &quot;#&quot;?.
        /// </summary>
        public static string People_Delete
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Er du sikker på at du vil slette \"#\"?";
                    default:
                        return "Are you sure you want to delete \"#\"?";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Delete the person.
        /// </summary>
        public static string People_Delete_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Slet samtykkeerklæringen";
                    default:
                        return "Delete the person";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Hide inactive.
        /// </summary>
        public static string People_HideInactive
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Skjul inaktive";
                    default:
                        return "Hide inactive";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Inactive.
        /// </summary>
        public static string People_InActive
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Inaktiv";
                    default:
                        return "Inactive";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Name.
        /// </summary>
        public static string People_Name
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Navn";
                    default:
                        return "Name";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Open a file with people.
        /// </summary>
        public static string People_Open_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Åben en fil med samtykkeerklæringer";
                    default:
                        return "Open a file with people";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Consent forms.
        /// </summary>
        public static string People_Page
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Samtykkeerklæringer";
                    default:
                        return "Consent forms";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Save the people to a file.
        /// </summary>
        public static string People_Save_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gem samtykkeerklæringer til den nuværende fil";
                    default:
                        return "Save the people to the current file";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Save the people to a file.
        /// </summary>
        public static string People_SaveAs_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gem samtykkeerklæringer til en ny fil";
                    default:
                        return "Save the people to a new file";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Search.
        /// </summary>
        public static string People_Search
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Søg";
                    default:
                        return "Search";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Search in the peoples names and comments.
        /// </summary>
        public static string People_Search_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Søg i samtykkeerklæringernes navne og kommentarer";
                    default:
                        return "Search in the peoples names and comments";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Hide/show people who are inactive.
        /// </summary>
        public static string People_ShowHide_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Skjul/vis samtykkeerklæringer der er inaktive";
                    default:
                        return "Hide/show people who are inactive";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Show inactive.
        /// </summary>
        public static string People_ShowInactive
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Vis inaktive";
                    default:
                        return "Show inactive";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Settings.
        /// </summary>
        public static string Settings
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Indstillinger";
                    default:
                        return "Settings";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Address.
        /// </summary>
        public static string Settings_AddressLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Adresse";
                    default:
                        return "Address";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Browse.
        /// </summary>
        public static string Settings_BrowseButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gennemse";
                    default:
                        return "Browse";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Delete program settings.
        /// </summary>
        public static string Settings_DeleteSettings
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Slet programindstillinger";
                    default:
                        return "Delete program settings";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Delete program settings.
        /// </summary>
        public static string Settings_DeleteSettings_Warning
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Er du sikker på at du vil slette alle system indstillinger til Timotheus? Dette inkluderer blandt andet: Sidst åbnet nøgle og evt. adgangskode, hvorvidt om programmet skal lukkes til systembakken, og om der skal kigges efter opdateringer.";
                    default:
                        return "Are you sure that you want to delete all system settings for Timotheus? This includes among other: Last opened key and its password, whether to close the program to system tray, and whether to look for updates.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Event settings.
        /// </summary>
        public static string Settings_Event
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Begivenhed indstillinger";
                    default:
                        return "Event settings";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Description.
        /// </summary>
        public static string Settings_EventDescription
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Beskrivelse";
                    default:
                        return "Description";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to End.
        /// </summary>
        public static string Settings_EventEnd
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Slut";
                    default:
                        return "End";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Start.
        /// </summary>
        public static string Settings_EventStart
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Start";
                    default:
                        return "Start";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Time.
        /// </summary>
        public static string Settings_EventTime
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Tidspunkt";
                    default:
                        return "Time";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Time.
        /// </summary>
        public static string Settings_HideToSystemTray
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Luk til systembakken";
                    default:
                        return "Close to system tray";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Association information.
        /// </summary>
        public static string Settings_InfoBox
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Foreningens information";
                    default:
                        return "Association information";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Language.
        /// </summary>
        public static string Settings_Language
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Sprog";
                    default:
                        return "Language";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Danish.
        /// </summary>
        public static string Settings_Language_Danish
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Dansk";
                    default:
                        return "Danish";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to English.
        /// </summary>
        public static string Settings_Language_English
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Engelsk";
                    default:
                        return "English";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Restart the program to change the language of the program.
        /// </summary>
        public static string Settings_LanguageChanged
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Genstart programmet for at skifte programmet sprog";
                    default:
                        return "Restart the program to change the language of the program";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Logo.
        /// </summary>
        public static string Settings_LogoLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Logo";
                    default:
                        return "Logo";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Look for updates.
        /// </summary>
        public static string Settings_LookForUpdates
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Led efter opdateringer";
                    default:
                        return "Look for updates";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Name.
        /// </summary>
        public static string Settings_NameLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Navn";
                    default:
                        return "Name";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Open program at startup (Without window)
        /// </summary>
        public static string Settings_OpenOnStartUp
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Åben når computeren tænder (Uden vindue)";
                    default:
                        return "Open at startup (Without window)";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Program settings.
        /// </summary>
        public static string Settings_Program
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Program indstillinger";
                    default:
                        return "Program settings";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Changes need to be uploaded.
        /// </summary>
        public static string SFTP_ChangedLocal
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ændringer skal uploades";
                    default:
                        return "Changes need to be uploaded";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Changes need to be downloaded.
        /// </summary>
        public static string SFTP_ChangedRemote
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ændringer skal downloades";
                    default:
                        return "Changes need to be downloaded";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Are you sure you want to make &quot;#1&quot; #2 with immediate effect? It is currently #3..
        /// </summary>
        public static string SFTP_ChangePermission
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Er du sikker på at du vil gøre \"#1\" #2 med øjeblikkelig virkning? Den er #3 lige nu.";
                    default:
                        return "Are you sure you want to make \"#1\" #2 with immediate effect? It is currently #3.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Delete.
        /// </summary>
        public static string SFTP_ContextMenu_Delete
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Slet";
                    default:
                        return "Delete";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to New folder.
        /// </summary>
        public static string SFTP_ContextMenu_NewFolder
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ny mappe";
                    default:
                        return "New folder";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Open.
        /// </summary>
        public static string SFTP_ContextMenu_Open
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Åben";
                    default:
                        return "Open";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Make private.
        /// </summary>
        public static string SFTP_ContextMenu_Private
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gør privat";
                    default:
                        return "Make private";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Make public.
        /// </summary>
        public static string SFTP_ContextMenu_Public
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gør offentlig";
                    default:
                        return "Make public";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Rename.
        /// </summary>
        public static string SFTP_ContextMenu_Rename
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Omdøb";
                    default:
                        return "Rename";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Show in file explorer.
        /// </summary>
        public static string SFTP_ContextMenu_ShowInExplorer
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Vis i stifinder";
                    default:
                        return "Show in file explorer";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Synchronize file.
        /// </summary>
        public static string SFTP_ContextMenu_Sync
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Synkronisér fil";
                    default:
                        return "Synchronize file";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Deletes the file locally and remotely.
        /// </summary>
        public static string SFTP_Delete_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Sletter filen lokalt og online";
                    default:
                        return "Deletes the file locally and remotely";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Deleted on computer.
        /// </summary>
        public static string SFTP_DeleteLocal
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Slettet på computer";
                    default:
                        return "Deleted on computer";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Deleted on server.
        /// </summary>
        public static string SFTP_DeleteRemote
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Slettet på server";
                    default:
                        return "Deleted on server";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Are you sure you want to delete &quot;#&quot; locally and remotely?.
        /// </summary>
        public static string SFTP_DeleteWarning
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Er du sikker på at du vil slette \"#\" lokalt og online?";
                    default:
                        return "Are you sure you want to delete \"#\" locally and remotely?";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to A folder with that name already exists..
        /// </summary>
        public static string SFTP_FolderExists
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "En mappe med det navn findes allerede";
                    default:
                        return "A folder with that name already exists.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Go back to the last folder.
        /// </summary>
        public static string SFTP_GoUp_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gå tilbage til den sidste mappe";
                    default:
                        return "Go back to the last folder";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Back.
        /// </summary>
        public static string SFTP_GoUpButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Tilbage";
                    default:
                        return "Back";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Synchronizing.
        /// </summary>
        public static string SFTP_Message
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Synkroniserer";
                    default:
                        return "Synchronizing";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to File name.
        /// </summary>
        public static string SFTP_NameColumn
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Filnavn";
                    default:
                        return "File name";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Hasn&apos;t been synced.
        /// </summary>
        public static string SFTP_NeverSynced
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ikke synkroniseret endnu";
                    default:
                        return "Hasn't been synced";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to New folder.
        /// </summary>
        public static string SFTP_NewFolder
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ny mappe";
                    default:
                        return "New folder";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Create a new folder.
        /// </summary>
        public static string SFTP_NewFolder_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Lav en ny mappe";
                    default:
                        return "Create a new folder";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Needs to be uploaded.
        /// </summary>
        public static string SFTP_NewLocal
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Skal uploades";
                    default:
                        return "Needs to be uploaded";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Needs to be downloaded.
        /// </summary>
        public static string SFTP_NewRemote
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Skal downloades";
                    default:
                        return "Needs to be downloaded";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Opens the file. If it is a directory, then the files in the directory are shown..
        /// </summary>
        public static string SFTP_Open_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Åbner den valgte fil. Hvis det er en mappe, går man ind i mappen.";
                    default:
                        return "Opens the file. If it is a directory, then the files in the directory are shown.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Files.
        /// </summary>
        public static string SFTP_Page
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Filer";
                    default:
                        return "Files";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The current folder path.
        /// </summary>
        public static string SFTP_Path_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Den nuværende mappes sti";
                    default:
                        return "The current folder path";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Change permission.
        /// </summary>
        public static string SFTP_Permission_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ændr tilladelse";
                    default:
                        return "Change permission";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Private.
        /// </summary>
        public static string SFTP_Private
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Privat";
                    default:
                        return "Private";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Public.
        /// </summary>
        public static string SFTP_Public
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Offentlig";
                    default:
                        return "Public";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Refresh the folder and look for updates.
        /// </summary>
        public static string SFTP_Refresh_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Genindlæs mappen og se om der er sket ændringer";
                    default:
                        return "Refresh the folder and look for updates";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Setup which folder to synchronize with.
        /// </summary>
        public static string SFTP_Setup_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Indstil hvilken mappe der skal synkroniseres med";
                    default:
                        return "Setup which folder to synchronize with";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Setup.
        /// </summary>
        public static string SFTP_SetupButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Opsætning";
                    default:
                        return "Setup";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Sync.
        /// </summary>
        public static string SFTPSetup_Sync
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Sync";
                    default:
                        return "Sync";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Sync.
        /// </summary>
        public static string SFTPSetup_SyncEvery
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Synkronisér hver";
                    default:
                        return "Synchronize every";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Sync.
        /// </summary>
        public static string SFTPSetup_Minutes
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "minutter";
                    default:
                        return "minutes";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Shows the file on the computer..
        /// </summary>
        public static string SFTP_ShowInExplorer_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Viser den valgte fil på computeren";
                    default:
                        return "Shows the file on the computer.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to File size.
        /// </summary>
        public static string SFTP_SizeColumn
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Filstørrelse";
                    default:
                        return "File size";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Synchronize all files and folders (Last synced #).
        /// </summary>
        public static string SFTP_Sync_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Synkroniser alle filer og mapper (Sidst synkroniseret #)";
                    default:
                        return "Synchronize all files and folders (Last synced #)";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Synchronize.
        /// </summary>
        public static string SFTP_SyncButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Synkronisér";
                    default:
                        return "Synchronize";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Synchronizes the file.
        /// </summary>
        public static string SFTP_SyncFile_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Synkronisér den valgte fil";
                    default:
                        return "Synchronizes the file";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Synchronizing folders.
        /// </summary>
        public static string SFTP_SyncWorker
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Synkroniserer filer";
                    default:
                        return "Synchronizing files";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Refresh.
        /// </summary>
        public static string SFTP_UpdateButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Opdatér";
                    default:
                        return "Refresh";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Setup.
        /// </summary>
        public static string SFTPSetup
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Opsætning";
                    default:
                        return "Setup";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Browse.
        /// </summary>
        public static string SFTPSetup_BrowseButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gennemsé";
                    default:
                        return "Browse";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        public static string SFTPSetup_CancelButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Annullér";
                    default:
                        return "Cancel";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Host.
        /// </summary>
        public static string SFTPSetup_HostLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Host";
                    default:
                        return "Host";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Local.
        /// </summary>
        public static string SFTPSetup_LocalDirectoryLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Lokal";
                    default:
                        return "Local";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Password.
        /// </summary>
        public static string SFTPSetup_PasswordLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Adgangskode";
                    default:
                        return "Password";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Port.
        /// </summary>
        public static string SFTPSetup_PortLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Port";
                    default:
                        return "Port";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Remote.
        /// </summary>
        public static string SFTPSetup_RemoteDirectoryLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Fjern";
                    default:
                        return "Remote";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Setup.
        /// </summary>
        public static string SFTPSetup_SetupButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Opsæt";
                    default:
                        return "Setup";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Username.
        /// </summary>
        public static string SFTPSetup_UsernameLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Brugernavn";
                    default:
                        return "Username";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Synchronize.
        /// </summary>
        public static string SyncCalendar
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Synkronisér";
                    default:
                        return "Synchronize";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to URL.
        /// </summary>
        public static string SyncCalendar_CalDAVLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "URL";
                    default:
                        return "URL";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        public static string SyncCalendar_CancelButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Annullér";
                    default:
                        return "Cancel";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Sync custom period.
        /// </summary>
        public static string SyncCalendar_CustomCalendarButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Synkronisér brugerdefineret periode";
                    default:
                        return "Sync custom period";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Destination.
        /// </summary>
        public static string SyncCalendar_Destination
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Destination";
                    default:
                        return "Destination";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Sync the entire calendar.
        /// </summary>
        public static string SyncCalendar_EntireCalendarButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Synkronisér hele kalenderen";
                    default:
                        return "Sync the entire calendar";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Another calendar.
        /// </summary>
        public static string SyncCalendar_NewCalendarButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "En anden kalender";
                    default:
                        return "Another calendar";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Password.
        /// </summary>
        public static string SyncCalendar_PasswordLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Adgangskode";
                    default:
                        return "Password";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Sync the period.
        /// </summary>
        public static string SyncCalendar_PeriodCalendarButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Synkronisér denne periode";
                    default:
                        return "Sync the period";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Settings.
        /// </summary>
        public static string SyncCalendar_Settings
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Indstillinger";
                    default:
                        return "Settings";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Sync.
        /// </summary>
        public static string SyncCalendar_SyncButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Synkronisér";
                    default:
                        return "Sync";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Current calendar.
        /// </summary>
        public static string SyncCalendar_UseExistingButton
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Nuværende kalender";
                    default:
                        return "Current calendar";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Username.
        /// </summary>
        public static string SyncCalendar_UsernameLabel
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Brugernavn";
                    default:
                        return "Username";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Synchronizing calendar.
        /// </summary>
        public static string SyncCalendar_Worker
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Synkroniserer kalender";
                    default:
                        return "Synchronizing calendar";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Text.
        /// </summary>
        public static string TextDialog
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Tekst";
                    default:
                        return "Text";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Edit.
        /// </summary>
        public static string ToolStrip_EditKey
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Redigér";
                    default:
                        return "Edit";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Make changes to the current key.
        /// </summary>
        public static string ToolStrip_EditKey_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Lav ændringer til den nuværende nøgle";
                    default:
                        return "Make changes to the current key";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Exit.
        /// </summary>
        public static string ToolStrip_Exit
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Luk";
                    default:
                        return "Exit";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to File.
        /// </summary>
        public static string ToolStrip_File
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Filer";
                    default:
                        return "File";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Help.
        /// </summary>
        public static string ToolStrip_Help
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Hjælp";
                    default:
                        return "Help";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Find information about the program.
        /// </summary>
        public static string ToolStrip_Help_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Find information omkring programmet";
                    default:
                        return "Find information about the program";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Key.
        /// </summary>
        public static string ToolStrip_Key
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Nøgle";
                    default:
                        return "Key";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Open.
        /// </summary>
        public static string ToolStrip_LoadKey
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Åben";
                    default:
                        return "Open";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to New.
        /// </summary>
        public static string ToolStrip_NewFile
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Ny";
                    default:
                        return "New";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Creates a new project.
        /// </summary>
        public static string ToolStrip_NewFile_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Lav et nyt projekt";
                    default:
                        return "Creates a new project";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Are you sure you want to create a new project? Unsaved progress will be lost..
        /// </summary>
        public static string ToolStrip_NewSecure
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Er du sikker på at du vil lave et nyt projekt? Alt der ikke er gemt, vil blive slettet.";
                    default:
                        return "Are you sure you want to create a new project? Unsaved progress will be lost.";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Open.
        /// </summary>
        public static string ToolStrip_OpenFile
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Åben";
                    default:
                        return "Open";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Find a key and open it.
        /// </summary>
        public static string ToolStrip_OpenKey_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Find en nøgle og åben den";
                    default:
                        return "Find a key and open it";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Save as.
        /// </summary>
        public static string ToolStrip_SaveAsKey
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gem som";
                    default:
                        return "Save as";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Save a copy of the current key to a specific location.
        /// </summary>
        public static string ToolStrip_SaveAsKey_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gem en kobi af den nuværende nøgle til et bestemt sted";
                    default:
                        return "Save a copy of the current key to a specific location";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Save.
        /// </summary>
        public static string ToolStrip_SaveFile
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gem";
                    default:
                        return "Save";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Save.
        /// </summary>
        public static string ToolStrip_SaveKey
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gem";
                    default:
                        return "Save";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Save the current key.
        /// </summary>
        public static string ToolStrip_SaveKey_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Gem den nuværende nøgle";
                    default:
                        return "Save the current key";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Here you can change the settings of the program.
        /// </summary>
        public static string ToolStrip_Settings_ToolTip
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Her kan du lave om på diverse indstillinger";
                    default:
                        return "Here you can change the settings of the program";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Open.
        /// </summary>
        public static string TrayIcon_Open
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Åben";
                    default:
                        return "Open";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Close.
        /// </summary>
        public static string TrayIcon_Close
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Luk";
                    default:
                        return "Close";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to There is a new update (#) available on the website: www.mjrj.dk.
        /// </summary>
        public static string UpdateDialog_Text
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Der er en ny opdatering (#) tilgængelig på hjemmesiden: www.mjrj.dk";
                    default:
                        return "There is a new update (#) available on the website: www.mjrj.dk";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Don't show again.
        /// </summary>
        public static string UpdateDialog_DontShowAgain
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Vis ikke igen";
                    default:
                        return "Don't show again";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Update.
        /// </summary>
        public static string UpdateDialog_Title
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Opdatering";
                    default:
                        return "Update";
                }
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Are you sure that you don't want messages about new updates? You can miss new features, better performance and security..
        /// </summary>
        public static string UpdateDialog_AreYouSure
        {
            get
            {
                switch (CurrentCulture)
                {
                    case Culture.daDK:
                        return "Er du sikker på at du ikke vil have beskeder om nye opdateringer? Du kan gå glip af nye funktioner, bedre ydeevne og sikkerhed.";
                    default:
                        return "Are you sure that you don't want messages about new updates? You can miss new features, better performance and security.";
                }
            }
        }
    }
}