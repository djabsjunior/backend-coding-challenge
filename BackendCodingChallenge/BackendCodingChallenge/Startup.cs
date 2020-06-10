using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendCodingChallenge.Calculators.CoordinateDistance;
using BackendCodingChallenge.Calculators.LevenshteinDistance;
using BackendCodingChallenge.Calculators.Scores;
using BackendCodingChallenge.Data.GeonamesAPI;
using BackendCodingChallenge.Data.Suggestions;
using BackendCodingChallenge.Validations.ParametersValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace BackendCodingChallenge
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<ICoordinateDistanceCalculator, CoordinateDistanceCalculator>();
            services.AddScoped<IGeonamesAPI, GeonamesAPI>();
            services.AddScoped<ILevenshteinDistanceCalculator, LevenshteinDistanceCalculator>();
            services.AddScoped<IScoresCalculator, ScoresCalculator>();
            services.AddScoped<ISuggestionsData, SuggestionsData>();
            services.AddScoped<ISuggestionsParameters, SuggestionsParameters>();
            services.AddSwaggerGen(sg => sg.SwaggerDoc("v1", new OpenApiInfo() { Title = "SuggestionsAPI", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "API to suggest large cities name"));
        }
    }
}
