using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arch.Core;
using Arch.Core.Extensions;

namespace Arch.Core;

/// <summary>
///     The <see cref="EntityInfo"/> struct
///     stores information about an <see cref="Entity"/> to quickly access its data and location.
/// </summary>
[SkipLocalsInit]
internal struct EntityInfo
{

    /// <summary>
    ///     A reference to its <see cref="Archetype"/>.
    /// </summary>
    public Archetype Archetype;

    /// <summary>
    ///     A reference to its <see cref="Slot"/>.
    /// </summary>
    public Slot Slot;

    /// <summary>
    ///     A reference to its version.
    /// </summary>
    public int Version;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EntityInfo"/> struct.
    /// </summary>
    /// <param name="archetype">Its <see cref="Archetype"/>.</param>
    /// <param name="slot">Its <see cref="Slot"/>.</param>
    /// <param name="version">Its version.</param>
    public EntityInfo(Archetype archetype, Slot slot, int version)
    {
        Archetype = archetype;
        Slot = slot;
        Version = version;
    }
}

/// <summary>
///     The <see cref="EntityInfo"/> struct
///     stores information about an <see cref="Entity"/> to quickly access its data and location.
/// </summary>
[SkipLocalsInit]
internal ref struct EntitySlot
{

    /// <summary>
    ///     A reference to its <see cref="Archetype"/>.
    /// </summary>
    public Archetype Archetype;

    /// <summary>
    ///     A reference to its <see cref="Slot"/>.
    /// </summary>
    public Slot Slot;


    /// <summary>
    ///     Initializes a new instance of the <see cref="EntityInfo"/> struct.
    /// </summary>
    /// <param name="archetype">Its <see cref="Archetype"/>.</param>
    /// <param name="slot">Its <see cref="Slot"/>.</param>
    /// <param name="version">Its version.</param>
    public EntitySlot(ref Archetype archetype, ref Slot slot)
    {
        Archetype = archetype;
        Slot = slot;
    }
}

/// <summary>
///     The <see cref="EntityInfoStorage"/> class
///     acts as an API and Manager to acess all <see cref="Entity"/> meta data and informations like its version, its <see cref="Archetype"/> or the <see cref="Chunk"/> it is in.
/// </summary>
internal class EntityInfoStorage
{

    /// <summary>
    ///     The <see cref="Entity"/> versions in an jagged array.
    /// </summary>
    private readonly JaggedArray<int> _versions;

    /// <summary>
    ///     The <see cref="Entity"/> <see cref="Archetype"/>s in an jagged array.
    /// </summary>
    private readonly JaggedArray<Archetype> _archetypes;

    /// <summary>
    ///     The <see cref="Entity"/> <see cref="Slot"/>s in an jagged array.
    /// </summary>
    private readonly JaggedArray<Slot> _slots;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EntityInfoStorage"/> class.
    /// </summary>
    public EntityInfoStorage()
    {
        _versions = new JaggedArray<int>(-1);
        _archetypes = new JaggedArray<Archetype>();
        _slots = new JaggedArray<Slot>(new Slot(-1,-1));
    }

    /// <summary>
    ///     Adds meta data of an <see cref="Entity"/> to the internal structure.
    /// </summary>
    /// <param name="id">The <see cref="Entity"/> id.</param>
    /// <param name="version">Its version.</param>
    /// <param name="archetype">Its <see cref="Archetype"/>.</param>
    /// <param name="slot">Its <see cref="Slot"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(int id, int version, Archetype archetype, Slot slot)
    {
        _versions.Add(id, version);
        _archetypes.Add(id, archetype);
        _slots.Add(id, slot);
    }

    /// <summary>
    ///     Checks whether an <see cref="Entity"/>s data exists in this <see cref="EntityInfoStorage"/> by its id.
    /// </summary>
    /// <param name="id">The <see cref="Entity"/>s id.</param>
    /// <returns>True if its data exists in here, false if not.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has(int id)
    {
        return _versions.TryGetValue(id, out _);
    }

    /// <summary>
    ///     Returns the <see cref="Archetype"/> of an <see cref="Entity"/> by its id.
    /// </summary>
    /// <param name="id">The <see cref="Entity"/>s id.</param>
    /// <returns>Its <see cref="Archetype"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Archetype GetArchetype(int id)
    {
        return _archetypes[id];
    }

    /// <summary>
    ///     Returns the <see cref="Slot"/> of an <see cref="Entity"/> by its id.
    /// </summary>
    /// <param name="id">The <see cref="Entity"/>s id.</param>
    /// <returns>Its <see cref="Slot"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref Slot GetSlot(int id)
    {
        return ref _slots[id];
    }

    /// <summary>
    ///     Returns the version of an <see cref="Entity"/> by its id.
    /// </summary>
    /// <param name="id">The <see cref="Entity"/>s id.</param>
    /// <returns>Its <see cref="Slot"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetVersion(int id)
    {
        return _versions[id];
    }

    /// <summary>
    ///     Trys to return the version of an <see cref="Entity"/> by its id.
    /// </summary>
    /// <param name="id">The <see cref="Entity"/>s id.</param>
    /// <param name="version">The <see cref="Entity"/>s version.</param>
    /// <returns>True if it exists, false if not.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetVersion(int id, out int version)
    {
        return _versions.TryGetValue(id, out version);
    }

    /// <summary>
    ///     Returns the <see cref="EntitySlot"/> of an <see cref="Entity"/> by its id.
    /// </summary>
    /// <param name="id">The <see cref="Entity"/>s id.</param>
    /// <returns>Its <see cref="EntitySlot"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntitySlot GetEntitySlot(int id)
    {
        return new EntitySlot(ref _archetypes[id], ref _slots[id]);
    }

    /// <summary>
    ///     Removes an enlisted <see cref="Entity"/> from this <see cref="EntityInfoStorage"/>.
    /// </summary>
    /// <param name="id">The <see cref="Entity"/>s id.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Remove(int id)
    {
        _archetypes.Remove(id);
        _slots.Remove(id);
        _versions.Remove(id);
    }

    /// <summary>
    ///     Moves an <see cref="Entity"/> to a new <see cref="Slot"/>, updates that reference.
    /// </summary>
    /// <param name="id">The <see cref="Entity"/> id.</param>
    /// <param name="slot">Its new <see cref="Slot"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Move(int id, Slot slot)
    {
        _slots[id] = slot;
    }

    /// <summary>
    ///     Moves an <see cref="Entity"/> to a new <see cref="Archetype"/> and a new <see cref="Slot"/>, updates that reference.
    /// </summary>
    /// <param name="id">The <see cref="Entity"/> id.</param>
    /// <param name="archetype">Its new <see cref="Archetype"/>.</param>
    /// <param name="slot">Its new <see cref="Slot"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Move(int id, Archetype archetype, Slot slot)
    {
        _archetypes[id] = archetype;
        _slots[id] = slot;
    }

    /// TODO : Find a cleaner way to break? One that does NOT require a branching?
    /// <summary>
    ///     Updates the <see cref="EntityInfo"/> and all entities that moved/shifted between the archetypes.
    /// </summary>
    /// <param name="archetype">The old <see cref="Archetype"/>.</param>
    /// <param name="archetypeSlot">The old <see cref="Slot"/> where the shift operation started.</param>
    /// <param name="newArchetype">The new <see cref="Archetype"/>.</param>
    /// <param name="newArchetypeSlot">The new <see cref="Slot"/> where the entities were shifted to.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Shift(Archetype archetype, Slot archetypeSlot, Archetype newArchetype, Slot newArchetypeSlot)
    {
        // Update the entityInfo of all copied entities.
        for (var chunkIndex = archetypeSlot.ChunkIndex; chunkIndex >= newArchetypeSlot.ChunkIndex; --chunkIndex)
        {
            // Get data
            ref var chunk = ref archetype.GetChunk(chunkIndex);
            ref var entityFirstElement = ref chunk.Entity(0);

            // Only move within the range, depening on which chunk we are at.
            var isStart = chunkIndex == archetypeSlot.ChunkIndex;
            var isEnd = chunkIndex == newArchetypeSlot.ChunkIndex;

            var upper = isStart ? archetypeSlot.Index : chunk.Size-1;
            var lower = isEnd ? newArchetypeSlot.Index : 0;

            for (var index = upper; index >= lower; --index)
            {
                ref readonly var entity = ref Unsafe.Add(ref entityFirstElement, index);

                // Calculate new entity slot based on its old slot.
                var entitySlot = new Slot(index, chunkIndex);
                var newSlot = Slot.Shift(entitySlot, archetype.EntitiesPerChunk, newArchetypeSlot, newArchetype.EntitiesPerChunk);

                // Update entity info
                Move(entity.Id, newArchetype, newSlot);
            }
        }
    }

    /// <summary>
    ///     Ensures the capacity of the underlaying arrays and resizes them properly.
    /// </summary>
    /// <param name="capacity"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void EnsureCapacity(int capacity)
    {
        _versions.EnsureCapacity(capacity);
        _archetypes.EnsureCapacity(capacity);
        _slots.EnsureCapacity(capacity);
    }

    /// <summary>
    ///     Trims the <see cref="EntityInfoStorage"/> and all of its underlaying arrays.
    ///     Releases memory.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void TrimExcess()
    {
        _versions.TrimExcess();
        _archetypes.TrimExcess();
        _slots.TrimExcess();
    }

    /// <summary>
    ///     Clears the <see cref="EntityInfoStorage"/> and all of its underlaying arrays.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear()
    {
        _versions.Clear();
        _archetypes.Clear();
        _slots.Clear();
    }

    /// <summary>
    ///     Returns a <see cref="EntityInfo"/> at an given index.
    /// </summary>
    /// <param name="id">The index.</param>
    public EntityInfo this[int id]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(_archetypes[id], _slots[id], _versions[id]);
    }
}

/// <summary>
///     The <see cref="JaggedArray"/> class
///     represents an jagged array that stores <see cref="EntityInfo"/> for quickly acessing it.
/// </summary>
internal class JaggedArray<T>
{
    /// <summary>
    ///     How large a chunk should be. This value will be a power of 2.
    /// </summary>
    private static readonly int _chunkSize;
    private static readonly int _chunkSizeMinusOne;

    /// <summary>
    ///     The jagged array storing the <see cref="EntityInfo"/>.
    /// </summary>
    private T[][] _entityInfos = Array.Empty<T[]>();

    /// <summary>
    ///     The fill value for new initialized arrays.
    /// </summary>
    private T filler;

    /// <summary>
    ///     The currently largest id inside this <see cref="JaggedArray"/>, for trimming purposes.
    /// </summary>
    private int _largestId;

    /// <summary>
    ///     Initializes the static values of <see cref="JaggedArray"/>.
    /// </summary>
    static JaggedArray()
    {
        var cpuL1CacheSize = 16_000; // In bytes
        var idealSize = cpuL1CacheSize / Unsafe.SizeOf<T>();

        _chunkSize = MathExtensions.RoundToPowerOfTwo(idealSize);
        _chunkSizeMinusOne = _chunkSize - 1;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="JaggedArray"/> class.
    /// </summary>
    public JaggedArray(T filler = default) : this(256, filler)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="JaggedArray"/> class.
    /// </summary>
    /// <param name="capacity">The initial capacity.</param>
    public JaggedArray(int capacity, T filler = default)
    {
        EnsureCapacity(capacity);
        this.filler = filler;
    }

    /// <summary>
    ///     Adds a new <see cref="EntityInfo"/> to a given index.
    /// </summary>
    /// <param name="id">The index.</param>
    /// <param name="entityInfo">The <see cref="EntityInfo"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(int id, T entityInfo)
    {
        this[id] = entityInfo;
    }

    /// <summary>
    ///     Removes an <see cref="EntityInfo"/> from a given index.
    ///     Replaces its value with the default one.
    /// </summary>
    /// <param name="id">The index.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Remove(int id)
    {
        this[id] = filler;
    }

    /// <summary>
    ///     Trys to return an <see cref="EntityInfo"/> from its index, if its set.
    /// </summary>
    /// <param name="id">The index.</param>
    /// <param name="entityInfo">The <see cref="EntityInfo"/>.</param>
    /// <returns>True if it was set, false if not.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetValue(int id, out T entityInfo)
    {
        // If the id is negative
        if (id < 0)
        {
            entityInfo = filler;
            return false;
        }

        IdToSlot(id, out var outerIndex, out var innerIndex);

        // If the item is outside the array. Then it definetly doesn't exist
        if (outerIndex > _entityInfos.Length)
        {
            entityInfo = filler;
            return false;
        }

        ref var item = ref _entityInfos[outerIndex][innerIndex];

        // If the item is the default then the nobody set its value.
        if (EqualityComparer<T>.Default.Equals(item, filler))
        {
            entityInfo = filler;
            return false;
        }

        entityInfo = item;
        return true;
    }

    /// <summary>
    ///     Converts the passed id to its inner and outer index ( or slot ) inside the <see cref="_entityInfos"/> array.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="outerIndex">The outer index.</param>
    /// <param name="innerIndex">The inner index.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void IdToSlot(int id, out int outerIndex, out int innerIndex)
    {
        Debug.Assert(id >= 0, "Id cannot be negative.");

        /* Instead of the '%' operator we can use logical '&' operator which is faster. But it requires the chunk size to be a power of 2. */
        outerIndex = id / _chunkSize;
        innerIndex = id & _chunkSizeMinusOne;
    }

    /// <summary>
    ///     Ensures the capacity of this <see cref="JaggedArray"/> and resizes it correctly.
    /// </summary>
    /// <param name="capacity">The new capacity.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void EnsureCapacity(int capacity)
    {
        if (capacity < _largestId)
        {
            return;
        }

        var currentSize = _entityInfos.Length;
        var desiredSize = (capacity / _chunkSize) + 1;

        Array.Resize(ref _entityInfos, desiredSize);

        // Create the new arrays.
        for (int i = currentSize; i < desiredSize; i++)
        {
            var array = new T[_chunkSize];
            _entityInfos[i] = new T[_chunkSize];
            Array.Fill(array, filler);
        }

        UpdateLargestId();
    }

    /// <summary>
    ///     Trims this <see cref="JaggedArray"/> and releases unused resources.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void TrimExcess()
    {
        var lastIndexWithNonDefaultValues = _entityInfos.Length - 1;
        for (var i = lastIndexWithNonDefaultValues; i >= 0; i--)
        {
            if (ArrayContainsNonDefaultValues(_entityInfos[i]))
            {
                break;
            }

            lastIndexWithNonDefaultValues = i - 1;
        }

        Array.Resize(ref _entityInfos, lastIndexWithNonDefaultValues + 1);
        UpdateLargestId();
    }

    /// <summary>
    ///     Clears this <see cref="JaggedArray"/> and sets all values to the default one.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear()
    {
        foreach (var array in _entityInfos)
        {
            Array.Fill(array, filler);
        }
    }

    /// <summary>
    ///     Updates the largest id.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void UpdateLargestId()
    {
        _largestId = _entityInfos.Length * _chunkSize;
    }

    /// <summary>
    ///     Checks if the passed <see cref="EntityInfo"/> array contains set values.
    /// </summary>
    /// <param name="array">The <see cref="EntityInfo"/> array to check.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool ArrayContainsNonDefaultValues(T[] array)
    {
        foreach (var item in array)
        {
            if (!EqualityComparer<T>.Default.Equals(item, filler))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///     Returns a reference to a <see cref="EntityInfo"/> at an given index.
    /// </summary>
    /// <param name="id">The index.</param>
    [SkipLocalsInit]
    public ref T this[int id]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            Debug.Assert(id >= 0, "Id cannot be negative");

            EnsureCapacity(id);
            IdToSlot(id, out var outerIndex, out var innerIndex);
            return ref _entityInfos[outerIndex][innerIndex];
        }
    }
}

