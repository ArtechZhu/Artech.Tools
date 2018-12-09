# 部署：
直接部署到IIS中，注意下.NET FRAMEWORK是否安装

# web.config配置：
注意调整以下几项配置：
- <add key="requireApiKey" value="true" />
- <add key="apiKey" value="40bf22ac8e42" />
- <add key="packagesPath" value="~/NugetPackages" />

# 制作、上传包：
url：