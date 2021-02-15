using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESTQuestFilling.ViewModel
{
    class FormsViewModel : PageViewModelBase
    {
        public FormsViewModel()
        {
            CompanyListCollection = new ObservableCollection<string>()
            {
                "Empty list"
            };
            CurrentInstitutionName = CompanyListCollection[0];
            CreateCompanyViewModelFromNameCommand = new DelegateCommand(CreateCompanyViewModelFromName);
        }

        public FormsViewModel(IEnumerable<string> companyList) : this()
        {
            CompanyListCollection = new ObservableCollection<string>(_companyListCollection.Concat(companyList));
        }

        public DelegateCommand CreateCompanyViewModelFromNameCommand { get; }

        private ObservableCollection<string> _companyListCollection;
        public ObservableCollection<string> CompanyListCollection
        {
            get => _companyListCollection;
            set
            {
                _companyListCollection = value;
                OnPropertyChanged();
            }
        }

        public void WriteCurrentCompanyFormsToXml()
        {
            CurrentCompanyViewModel.WriteCheckpointsToFiles();
        }

        public string CurrentInstitutionName { get; set; }

        private void CreateCompanyViewModelFromName(object parameter)
        {
            //CurrentCompanyViewModel = new CompanyViewModel(parameter.ToString(), DatabasePath);
        }
        
        private CompanyViewModel _currentCompanyViewModel;
        public CompanyViewModel CurrentCompanyViewModel
        {
            get => _currentCompanyViewModel;
            set
            {
                _currentCompanyViewModel = value;
                CurrentInstitutionName = value.Name;
                OnPropertyChanged(nameof(CurrentCompanyViewModel));
                OnPropertyChanged(nameof(CurrentInstitutionName));
            }
        }
    }
}
