using Crowdfund.Core.Models;
using System;

namespace Crowdfund.Web
{
    public static class Globals
    {
        public static int? UserId { get; set; }
        public static readonly Category[] Categories = (Category[]) Enum.GetValues(typeof(Category));
    }
}