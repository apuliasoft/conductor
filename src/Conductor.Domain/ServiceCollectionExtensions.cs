﻿using Conductor.Domain.Interfaces;
using Conductor.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Conductor.Domain
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddSingleton<IDefinitionService, DefinitionService>();
            services.AddSingleton<IWorkflowLoader, WorkflowLoader>();
            services.AddSingleton<ICustomStepService, CustomStepService>();
            services.AddSingleton<IExpressionEvaluator, ExpressionEvaluator>();
            services.AddSingleton<IEventBulkService, EventBulkService>();
            services.AddSingleton<IWorkflowBulkService, WorkflowBulkService>();
            services.AddTransient<CustomStep>();
        }
    }
}
