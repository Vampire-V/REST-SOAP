using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using DemoSAP.Services;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging(logging =>
{
    logging.AddFile("Logs/myapp-{Date}.txt");
});

// เพิ่มการตั้งค่า CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});

// เพิ่ม Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// อ่านค่า BaseAddress จาก appsettings.json
var baseAddress = builder.Configuration["SapService:BaseAddress"];
if (string.IsNullOrEmpty(baseAddress))
{
    throw new ArgumentNullException("BaseAddress is not configured in appsettings.json");
}

// ลงทะเบียน HttpClient และ SapService
builder.Services.AddHttpClient<ISapService, SapService>(client =>
{
    client.BaseAddress = new Uri(baseAddress);
});

var app = builder.Build();

// เปิดใช้งาน CORS
app.UseCors("AllowAllOrigins");

// เปิดใช้งาน Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo SAP API V1");
    options.RoutePrefix = ""; // ให้ Swagger แสดงที่ root URL
});

// เปิดใช้งาน Routing
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    // เพิ่ม SOAP Endpoint
    endpoints.UseSoapEndpoint<ISapService>(
        "/SoapService.svc", // Path ของ SOAP Endpoint
        new SoapEncoderOptions
        {
            MessageVersion = MessageVersion.Soap11, // ใช้ SOAP 1.1
            WriteEncoding = Encoding.UTF8, // ใช้ UTF-8
            ReaderQuotas = XmlDictionaryReaderQuotas.Max, // อนุญาต Reader Quotas สูงสุด
            OverwriteResponseContentType = true, // บังคับเปลี่ยน Content-Type ใน Response
            BindingName = "BasicHttpBinding", // ตั้งค่า Binding Name
            PortName =
                "SoapPort" // ตั้งค่า Port Name
            ,
        },
        SoapSerializer.DataContractSerializer // ใช้ DataContractSerializer
    );
});

// เปิดใช้งาน REST API Controllers
app.MapControllers();

await app.RunAsync();
