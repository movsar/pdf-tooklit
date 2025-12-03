namespace CornerstoneApiServices.Models.Learning
{
    public class LearningObject
    {
        public string ActorID { get; set; }
        public List<Availability> Availabilities { get; set; }
        public List<AvailableLanguage> AvailableLanguages { get; set; }
        public List<Competency> Competencies { get; set; }
        public List<CustomFieldItem> CustomFieldItems { get; set; }
        public EventItem EventItem { get; set; }
        public MaterialItem MaterialItem { get; set; }
        public OnlineItem OnlineItem { get; set; }
        public List<PostWork> PostWorks { get; set; }
        public List<PreRequisiteOption> PreRequisiteOptions { get; set; }
        public List<PreWork> PreWorks { get; set; }
        public SessionItem SessionItem { get; set; }
        public List<SkillTitle> SkillTitles { get; set; }
        public List<SubjectTitle> SubjectTitles { get; set; }
        public int TrainingHours { get; set; }
        public int TrainingMins { get; set; }
        public int Type { get; set; }


        public class Competency
        {
            public string LoId { get; set; }
        }

        public class CustomFieldItem
        {
            public string Tag { get; set; }
            public string Value { get; set; }
        }

        public class PostWork
        {
            public string LoId { get; set; }
        }

        public class PreRequisite
        {
            public string LoId { get; set; }
        }

        public class PreRequisiteOption
        {
            public List<PreRequisite> PreRequisites { get; set; }
        }

        public class PreWork
        {
            public string LoId { get; set; }
        }

        public class SkillTitle
        {
            public string Title { get; set; }
        }

        public class SubjectTitle
        {
            public string Title { get; set; }
        }
    }
}
