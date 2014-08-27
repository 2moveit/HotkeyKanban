using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace KCT.HotkeyKanban.UI.ViewModels
{
    public class CardViewModel : Screen
    {
        private readonly Task task;
        private string description;
        private Guid id;
        private int shortId;

        public CardViewModel(Task task, int shortId)
        {
            this.task = task;
            Id = task.Id;
            Description = task.Description;
            ShortId = shortId;
        }

        public int ShortId
        {
            get { return shortId; }
            set
            {
                if (value == shortId) return;
                shortId = value;
                NotifyOfPropertyChange(() => ShortId);
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (value == description) return;
                description = value;
                NotifyOfPropertyChange(() => Description);
            }
        }

        public Guid Id
        {
            get { return id; }
            set
            {
                if (value.Equals(id)) return;
                id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }
    }
}
