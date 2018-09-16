using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.IoC;

namespace MVxIRx.Core
{
    public static class MvxTypeExtensions
    {
        //TODO : add interfaces inside concrete type pairs
        /*
        public static IEnumerable<MvvmCross.IoC.MvxTypeExtensions.ServiceTypeAndImplementationTypePair> AsTypesAndInterfaces(this IEnumerable<Type> types)
        {
            var t = types.AsTypes();
            var i = types.AsInterfaces();


            return u;
        }
        */

        /*
        public static IEnumerable<MvvmCross.IoC.MvxTypeExtensions.ServiceTypeAndImplementationTypePair> AsTypes(this IEnumerable<Type> types)
            => types.Select(t => new MvvmCross.IoC.MvxTypeExtensions.ServiceTypeAndImplementationTypePair(new List<Type>() { t }, t));

        public static IEnumerable<MvvmCross.IoC.MvxTypeExtensions.ServiceTypeAndImplementationTypePair> AsInterfaces(this IEnumerable<Type> types)
            => types.Select(t => new MvvmCross.IoC.MvxTypeExtensions.ServiceTypeAndImplementationTypePair(t.GetInterfaces().ToList(), t));

        public static IEnumerable<MvvmCross.IoC.MvxTypeExtensions.ServiceTypeAndImplementationTypePair> AsInterfaces(this IEnumerable<Type> types, params Type[] interfaces)
        {
            // optimisation - if we have 3 or more interfaces, then use a dictionary
            if (interfaces.Length >= 3)
            {
                var lookup = interfaces.ToDictionary(x => x, x => true);
                return
                    types.Select(
                        t =>
                            new MvvmCross.IoC.MvxTypeExtensions.ServiceTypeAndImplementationTypePair(
                                t.GetInterfaces().Where(iface => lookup.ContainsKey(iface)).ToList(), t));
            }
            else
            {
                return
                    types.Select(
                        t =>
                            new MvvmCross.IoC.MvxTypeExtensions.ServiceTypeAndImplementationTypePair(
                                t.GetInterfaces().Where(iface => interfaces.Contains(iface)).ToList(), t));
            }
        }*/
    }
}
