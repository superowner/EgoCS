﻿using UnityEngine;
using System.Collections.Generic;

public class EgoSystem<C1, C2, C3, C4> : IEgoSystem
    where C1 : Component
    where C2 : Component
    where C3 : Component
    where C4 : Component
{
#if UNITY_EDITOR
    bool _enabled = true;
    public bool enabled { get { return _enabled; } set { _enabled = value; } }
#endif

    protected BitMask _mask = new BitMask( ComponentIDs.GetCount() );

    protected Dictionary<EgoComponent, EgoBundle<C1, C2, C3, C4>> _bundles = new Dictionary<EgoComponent, EgoBundle<C1, C2, C3, C4>>();
    public Dictionary<EgoComponent, EgoBundle<C1, C2, C3, C4>>.ValueCollection bundles { get { return _bundles.Values; } }

    public EgoSystem()
    {
        _mask[ComponentIDs.Get( typeof( C1 ) )] = true;
        _mask[ComponentIDs.Get( typeof( C2 ) )] = true;
        _mask[ComponentIDs.Get( typeof( C3 ) )] = true;
        _mask[ComponentIDs.Get( typeof( C4 ) )] = true;
        _mask[ComponentIDs.Get( typeof( EgoComponent ) )] = true;

        // Attach built-in Event Handlers
        EgoEvents<AddedGameObject>.AddHandler( Handle );
        EgoEvents<DestroyedGameObject>.AddHandler( Handle );
        EgoEvents<AddedComponent<C1>>.AddHandler( Handle );
        EgoEvents<AddedComponent<C2>>.AddHandler( Handle );
        EgoEvents<AddedComponent<C3>>.AddHandler( Handle );
        EgoEvents<AddedComponent<C4>>.AddHandler( Handle );
        EgoEvents<DestroyedComponent<C1>>.AddHandler( Handle );
        EgoEvents<DestroyedComponent<C2>>.AddHandler( Handle );
        EgoEvents<DestroyedComponent<C3>>.AddHandler( Handle );
        EgoEvents<DestroyedComponent<C4>>.AddHandler( Handle );
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
            var component2 = egoComponent.GetComponent<C2>();
            var component3 = egoComponent.GetComponent<C3>();
            var component4 = egoComponent.GetComponent<C4>();
            CreateBundle( egoComponent, component1, component2, component3, component4 );
        }
    }

    protected void CreateBundle( EgoComponent egoComponent, C1 component1 )
    {
        if( Ego.CanUpdate( _mask, egoComponent.mask ) )
        {
            var component2 = egoComponent.GetComponent<C2>();
            var component3 = egoComponent.GetComponent<C3>();
            var component4 = egoComponent.GetComponent<C4>();
            CreateBundle( egoComponent, component1, component2, component3, component4 );
        }
    }

    protected void CreateBundle( EgoComponent egoComponent, C2 component2 )
    {
        if( Ego.CanUpdate( _mask, egoComponent.mask ) )
        {
            var component1 = egoComponent.GetComponent<C1>();
            var component3 = egoComponent.GetComponent<C3>();
            var component4 = egoComponent.GetComponent<C4>();
            CreateBundle( egoComponent, component1, component2, component3, component4 );
        }
    }

    protected void CreateBundle( EgoComponent egoComponent, C3 component3 )
    {
        if( Ego.CanUpdate( _mask, egoComponent.mask ) )
        {
            var component1 = egoComponent.GetComponent<C1>();
            var component2 = egoComponent.GetComponent<C2>();
            var component4 = egoComponent.GetComponent<C4>();
            CreateBundle( egoComponent, component1, component2, component3, component4 );
        }
    }

    protected void CreateBundle( EgoComponent egoComponent, C4 component4 )
    {
        if( Ego.CanUpdate( _mask, egoComponent.mask ) )
        {
            var component1 = egoComponent.GetComponent<C1>();
            var component2 = egoComponent.GetComponent<C2>();
            var component3 = egoComponent.GetComponent<C3>();
            CreateBundle( egoComponent, component1, component2, component3, component4 );
        }
    }

    protected void CreateBundle( EgoComponent egoComponent, C1 component1, C2 component2, C3 component3, C4 component4 )
    {
        var bundle = new EgoBundle<C1, C2, C3, C4>( egoComponent, component1, component2, component3, component4 );
        _bundles[egoComponent] = bundle;
    }

    protected void RemoveBundle( EgoComponent egoComponent )
    {
        _bundles.Remove( egoComponent );
    }

    public virtual void Start()
    {
        foreach( var bundle in bundles )
        {
            Start( bundle.egoComponent, bundle.component1, bundle.component2, bundle.component3, bundle.component4 );
        }
    }

    public virtual void Update()
    {
        foreach( var bundle in bundles )
        {
            Update( bundle.egoComponent, bundle.component1, bundle.component2, bundle.component3, bundle.component4 );
        }
    }

    public virtual void FixedUpdate()
    {
        foreach( var bundle in bundles )
        {
            FixedUpdate( bundle.egoComponent, bundle.component1, bundle.component2, bundle.component3, bundle.component4 );
        }
    }

    public virtual void Start( EgoComponent egoComponent, C1 component1, C2 component2, C3 component3, C4 component4 ) { }

    public virtual void Update( EgoComponent egoComponent, C1 component1, C2 component2, C3 component3, C4 component4 ) { }

    public virtual void FixedUpdate( EgoComponent egoComponent, C1 component1, C2 component2, C3 component3, C4 component4 ) { }

    //
    // Event Handlers
    //

    void Handle( AddedGameObject e )
    {
        CreateBundle( e.egoComponent );
    }

    void Handle( DestroyedGameObject e )
    {
        _bundles.Remove( e.egoComponent );
    }

    void Handle( AddedComponent<C1> e )
    {
        CreateBundle( e.egoComponent, e.component );
    }

    void Handle( AddedComponent<C2> e )
    {
        CreateBundle( e.egoComponent, e.component );
    }

    void Handle( AddedComponent<C3> e )
    {
        CreateBundle( e.egoComponent, e.component );
    }

    void Handle( AddedComponent<C4> e )
    {
        CreateBundle( e.egoComponent, e.component );
    }

    void Handle( DestroyedComponent<C1> e )
    {
        RemoveBundle( e.egoComponent );
    }

    void Handle( DestroyedComponent<C2> e )
    {
        RemoveBundle( e.egoComponent );
    }

    void Handle( DestroyedComponent<C3> e )
    {
        RemoveBundle( e.egoComponent );
    }

    void Handle( DestroyedComponent<C4> e )
    {
        RemoveBundle( e.egoComponent );
    }
}
