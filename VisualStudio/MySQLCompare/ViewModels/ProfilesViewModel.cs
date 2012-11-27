using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using MySQLCompare.Models;
using MySQLCompare.Architecture;

namespace MySQLCompare.ViewModels
{
    class ProfilesViewModel : ViewModelBase
    {
        private ObservableCollection<ProfileItemViewModel> _profiles;
        private ProfileItemViewModel _selectedProfile;

        public ProfilesViewModel()
            :this(new List<Profile>())
        {
        }

        public ProfilesViewModel(List<Profile> profiles)
        {
            ObservableCollection<ProfileItemViewModel> profileModels = new ObservableCollection<ProfileItemViewModel>();
            foreach (Profile profile in profiles)
            {
                profileModels.Add(new ProfileItemViewModel(profile));
            }
            Profiles = profileModels;
        }

        public ObservableCollection<ProfileItemViewModel> Profiles
        {
            get
            {
                if (_profiles == null)
                {
                    _profiles = new ObservableCollection<ProfileItemViewModel>();
                }

                return _profiles;
            }

            set
            {
                _profiles = value;
            }
        }

        public ProfileItemViewModel SelectedProfile
        {
            get
            {
                return _selectedProfile;
            }

            set
            {
                _selectedProfile = value;
                OnPropertyChanged("SelectedProfile");
            }
        }

        #region Commands

        // The following commands are stubs, not yet implemented

        #region Edit Profile Command
        // Purpose: Edit a saved database profile.
        public AppCommand EditProfileCommand
        {
            get;
            set;
        }

        public void OnEditProfileCommand(object sender)
        {
            System.Diagnostics.Debug.WriteLine("Doing onEditProfileCommand...");
        }

        public bool CanPerformEditProfileCommand(object sender)
        {
            System.Diagnostics.Debug.WriteLine("Doing canPerform...");

            if (SelectedProfile == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region Remove Profile Command

        // "Remove Profile" command not yet implemented.
        // Purpose: Remove the selected database profile.
        public AppCommand RemoveProfileCommand
        {
            get;
            set;
        }

        #endregion

        #endregion
    }
}
