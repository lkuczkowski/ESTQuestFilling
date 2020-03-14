using ESTQuestFilling.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace ESTQuestFilling.ViewModel
{
    class CompanyViewModel : BindableBase
    {
        private readonly Company _institution;

        public CompanyViewModel(Company institution)
        {
            _institution = institution;
            CheckpointViewModelsList = new ObservableCollection<CheckpointViewModel>(_institution.CheckpointsList.Select(n=> new CheckpointViewModel(n)));
        }

        public string Name => _institution.Name;

        public ObservableCollection<CheckpointViewModel> CheckpointViewModelsList { get; set; }

        public void WriteCheckpointsToFiles() => _institution.WriteInstitutionCodeFiles();
    }
}
