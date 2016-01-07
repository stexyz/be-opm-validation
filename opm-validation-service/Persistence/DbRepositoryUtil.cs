using opm_validation_service.Persistence.ORM;

namespace opm_validation_service.Persistence
{
    public static class DbRepositoryUtil
    {
        private const string PositiveTestData = "OpmRepoSampleData.csv";

        public static void RecreateDatabase()
        {
            BE_Opm context = new BE_Opm();
            if (context.Database.Exists()) {
                context.Database.Delete();
            }
            context.Database.Create();
        }

        public static void FillSampleOpm(string path)
        {
            IOpmRepository repo = new OpmDbRepository();
            OpmRepoFiller.Fill(repo, path);
        }
    }
}