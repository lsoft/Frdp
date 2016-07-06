using System;
using Ninject.Syntax;

namespace Frdp.Server.CompositionRoot
{
    public static class NinjectExtender
    {
        public static IBindingInNamedWithOrOnSyntax<T> WhenTypeAndConstructorArgumentNamed<T>(
            this IBindingWhenSyntax<T> bws,
            Type type,
            string constructorArgumentName
            )
        {
            if (bws == null)
            {
                throw new ArgumentNullException("bws");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (constructorArgumentName == null)
            {
                throw new ArgumentNullException("constructorArgumentName");
            }

            return
                bws
                    .When(request =>
                    {
                        var result = false;

                        if (request != null)
                        {
                            if (request.Target != null)
                            {
                                if (request.Target.Name == constructorArgumentName)
                                {
                                    if (request.Target.Member != null)
                                    {
                                        if (request.Target.Member.DeclaringType == type)
                                        {
                                            result = true;
                                        }
                                    }
                                }
                            }
                        }

                        return result;
                    });
        }
    }
}