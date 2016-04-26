﻿using UnityEngine;
using System.Collections.Generic;

public class EgoSystem<C1> : IEgoSystem
    where C1 : Component
{
#if UNITY_EDITOR
    bool _enabled = true;
    public bool enabled { get { return _enabled; } set { _enabled = value; } }
#endif

    protected BitMask _mask = new BitMask( ComponentIDs.GetCount() );

    protected Dictionary<EgoComponent, EgoBundle<C1>> _bundles = new Dictionary<EgoComponent, EgoBundle<C1>>();
    public Dictionary<EgoComponent, EgoBundle<C1>>.ValueCollection bundles { get { return _bundles.Values; } }

    public EgoSystem()
    {
        _mask[ComponentIDs.Get( typeof( C1 ) )] = true;
        _mask[ComponentIDs.Get( typeof( EgoComponent ) )] = true;

        // Attach built-in Event Handlers
        EgoEvents<AddedGameObject>.AddHandler( Handle );
        EgoEvents<DestroyedGameObject>.AddHandler( Handle );
        EgoEvents<AddedComponent<C1>>.AddHandler( Handle );
        EgoEvents<DestroyedComponent<C1>>.AddHandler( Handle );
    }

    public void CreateBundles( EgoComponent[] egoComponents )
    {
        foreach( var egoComponent in egoComponents )
        {
            CreateBundle( egoComponent );
        }
    }

    protected void CreateBundle( EgoComponent egoComponent )
    {
        if( Ego.CanUpdate( _mask, egoComponent.mask ) )
        {
            var component1 = egoComponent.GetComponent<C1>();
            CreateBundle( egoComponent, component1 );
        }
    }

    protected void CreateBundle( EgoComponent egoComponent, C1 component1 )
    {
        var bundle = new EgoBundle<C1>( egoComponent, component1 );
        _bundles[ egoComponent ] = bundle;
    }

    protected void RemoveBundle( EgoComponent egoComponent )
    {
        _bundles.Remove( egoComponent );
    }

    public virtual void Start()
    {
        foreach( var bundle in bundles )
        {
            Start( bundle.egoComponent, bundle.component1 );
        }
    }

    public virtual void Update()
    {
        foreach( var bundle in bundles )
        {
            Update( bundle.egoComponent, bundle.component1 );
        }
    }

    public virtual void FixedUpdate()
    {
        foreach( var bundle in bundles )
        {
            FixedUpdate( bundle.egoComponent, bundle.component1 );
        }
    }
    public virtual void Start( EgoComponent egoComponent, C1 component1 ) { }

    public virtual void Update( EgoComponent egoComponent, C1 component1 ) { }

    public virtual void FixedUpdate( EgoComponent egoComponent, C1 component1 ) { }

    //
    // Event Handlers
    //

    void Handle( AddedGameObject e )
    {
        CreateBundle( e.egoComponent );
    }

    void Handle( DestroyedGameObject e )
    {
        RemoveBundle( e.egoComponent );
    }

    void Handle( AddedComponent<C1> e )
    {
        CreateBundle( e.egoComponent, e.component );
    }

    void Handle( DestroyedComponent<C1> e )
    {
        RemoveBundle( e.egoComponent );
    }
}
