namespace ToDoList.Test;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Domain.Models;
using System.ComponentModel;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;


using Microsoft.AspNetCore.Mvc;

public static class ActionResultExtensions
{
    public static T? GetValue<T>(this ActionResult<T> result) => result.Result is null
        ? result.Value
        : (T?)(result.Result as ObjectResult)?.Value;
}
