namespace Wscad.VectorGraphicViewer.Infrastructure.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using Wscad.VectorGraphicViewer.Domain.Contracts;
    using Wscad.VectorGraphicViewer.Domain.Contracts.Repositories;
    using Wscad.VectorGraphicViewer.Domain.Entities;
    using Wscad.VectorGraphicViewer.Domain.Enums;

    /// <summary>
    /// Repository with a simple in-memory cache shared by GetAll and GetByType.
    /// Keeps a completeness flag so GetAll knows when to fetch everything.
    /// </summary>
    public class PrimitiveRepository : IPrimitiveRepository
    {
        private readonly IPrimitivesDataSource _source;

        // Single in-memory cache (mutable internally).
        private List<Primitive>? _cache;

        // Indicates whether _cache currently represents the full dataset.
        private bool _hasAll;

        public PrimitiveRepository(IPrimitivesDataSource source)
        {
            _source = source;
            _hasAll = false; // start as partial/unknown
        }

        public IReadOnlyList<Primitive> GetAll()
        {
            // Load all only if cache is empty or known-partial
            if (_cache is null || !_hasAll)
            {
                _cache = _source.GetAll().ToList();
                _hasAll = true; // now we have the full set
            }

            return _cache;
        }

        public Primitive? GetByType(PrimitiveTypeEnum type)
        {
            // 1) Try from cache first
            if (_cache is not null)
            {
                Primitive? hit = _cache.FirstOrDefault(p => p.Kind == type);
                if (hit is not null)
                    return hit;
            }

            // 2) Ask the data source for this specific type
            Primitive? fromSource = _source.GetByType(type);
            if (fromSource is null)
                return null;

            // 3) Seed or append to cache, but mark as partial (we didn't fetch all)
            if (_cache is null)
                _cache = new List<Primitive> { fromSource };
            else
                _cache.Add(fromSource);

            _hasAll = false; // cache is partial again (only some items are known)
            return fromSource;
        }
    }
}