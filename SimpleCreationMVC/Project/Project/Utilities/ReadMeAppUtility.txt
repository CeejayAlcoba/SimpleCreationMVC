
Sample GetConnectionString using App Utility
ICOnfigurationRoot _config = new AppUlitity().configuration;
_config.GetConnectionString("DefaultConnection");

Sample get item in appsetting.json
ICOnfigurationRoot _config = new AppUlitity().configuration;
_config["JWT:Secret"];
