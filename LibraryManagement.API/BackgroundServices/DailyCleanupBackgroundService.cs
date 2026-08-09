namespace LibraryManagement.API.BackgroundServices
{
    public class DailyCleanupBackgroundService : BackgroundService
    {
        private readonly ILogger<DailyCleanupBackgroundService> _logger;
        private readonly string _uploadsFolder;

        public DailyCleanupBackgroundService(ILogger<DailyCleanupBackgroundService> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _uploadsFolder = Path.Combine(env.ContentRootPath, "Uploads");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Daily cleaning service has started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Running scheduled task: Checking for expired files...");

                    if (Directory.Exists(_uploadsFolder))
                    {
                        var files = Directory.GetFiles(_uploadsFolder);
                        foreach (var file in files)
                        {
                            var fileInfo = new FileInfo(file);

                            if (fileInfo.CreationTime < DateTime.Now.AddDays(-1))
                            {
                                fileInfo.Delete();
                                _logger.LogInformation("Old file deleted: {FileName}", fileInfo.Name);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during cleaning.");
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
