using ESTQuestFilling.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace ESTQuestFilling.ViewModel
{
    class CheckpointViewModel : BindableBase
    {
        private readonly Checkpoint _checkpoint;

        public CheckpointViewModel(Checkpoint checkpoint)
        {
            _checkpoint = checkpoint;
            QuestionListViewModel = new ObservableCollection<QuestionViewModel>(_checkpoint.QuestionsList.Select(n => new QuestionViewModel(n)));
        }

        public int ID => _checkpoint.Id;

        public string CheckpointName => $"Checkpoint (id: {_checkpoint.Id}) {_checkpoint.Name}";

        public ObservableCollection<QuestionViewModel> QuestionListViewModel { get; }
    }
}
