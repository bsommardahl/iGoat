using System;
using AutoMapper;

namespace iGoat.Service
{
    public class AutoMapperMappingEngine : IMappingEngine
    {
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }

        public TDestination DynamicMap<TSource, TDestination>(TSource source)
        {
            throw new NotImplementedException();
        }

        public TDestination DynamicMap<TDestination>(object source)
        {
            throw new NotImplementedException();
        }

        public void DynamicMap<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new NotImplementedException();
        }

        public void DynamicMap(object source, object destination, Type sourceType, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public object DynamicMap(object source, Type sourceType, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            throw new NotImplementedException();
        }
    }
}