using LibraryManagement.DAL;
using LibraryManagement.Business;
using FluentValidation; 
using LibraryManagement.Business.Validators.Book; 
using LibraryManagement.API.Middlewares; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDalServices(builder.Configuration);
builder.Services.AddBusinessServices();


builder.Services.AddValidatorsFromAssemblyContaining<BookCreateDtoValidator>();

// ==========================================

var app = builder.Build();


app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();