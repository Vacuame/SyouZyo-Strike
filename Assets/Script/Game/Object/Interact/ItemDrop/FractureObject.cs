using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class FractureObject : MonoBehaviour
{
    public FractureOptions fractureOptions;
    public RefractureOptions refractureOptions;
    public CallbackOptions callbackOptions;

    /// <summary>
    /// The number of times this fragment has been re-fractured.
    /// </summary>
    [HideInInspector]
    public int currentRefractureCount = 0;

    /// <summary>
    /// Collector object that stores the produced fragments
    /// </summary>
    private GameObject fragmentRoot;

    private void Awake()
    {
        EventManager.Instance.AddListener<HitInfo>("Hit" + gameObject.GetInstanceID(), OnHit);
    }

    private HitInfo hitInfo;
    private void OnHit(HitInfo info)
    {
        hitInfo = info;
        this.ComputeFracture();
        //callbackOptions.onCompleted += AddHitForce;
    }

    public void AddHitForceOnComplete()
    {

    }

    void OnValidate()
    {
        if (this.transform.parent != null)
        {
            var scale = this.transform.parent.localScale;
            if ((scale.x != scale.y) || (scale.x != scale.z) || (scale.y != scale.z))
            {
                Debug.LogWarning($"Warning: Parent transform of fractured object must be uniformly scaled in all axes or fragments will not render correctly.", this.transform);
            }
        }
    }

    private void ComputeFracture()
    {
        var mesh = this.GetComponent<MeshFilter>().sharedMesh;

        if (mesh != null)
        {
            // If the fragment root object has not yet been created, create it now
            if (this.fragmentRoot == null)
            {
                // Create a game object to contain the fragments
                this.fragmentRoot = new GameObject($"{this.name}Fragments");
                this.fragmentRoot.transform.SetParent(this.transform.parent);

                // Each fragment will handle its own scale
                this.fragmentRoot.transform.position = this.transform.position;
                this.fragmentRoot.transform.rotation = this.transform.rotation;
                this.fragmentRoot.transform.localScale = Vector3.one;
            }

            var fragmentTemplate = CreateFragmentTemplate();

            if (fractureOptions.asynchronous)
            {
                StartCoroutine(Fragmenter.FractureAsync(
                    this.gameObject,
                    this.fractureOptions,
                    fragmentTemplate,
                    this.fragmentRoot.transform,
                    () =>
                    {
                        // Done with template, destroy it
                        GameObject.Destroy(fragmentTemplate);

                        // Deactivate the original object
                        this.gameObject.SetActive(false);

                        // Fire the completion callback
                        if ((this.currentRefractureCount == 0) ||
                            (this.currentRefractureCount > 0 && this.refractureOptions.invokeCallbacks))
                        {
                            if (callbackOptions.onCompleted != null)
                            {
                                callbackOptions.onCompleted.Invoke();
                            }
                        }
                    }
                ));
            }
            else
            {
                Fragmenter.Fracture(this.gameObject,
                                    this.fractureOptions,
                                    fragmentTemplate,
                                    this.fragmentRoot.transform);

                // Done with template, destroy it
                GameObject.Destroy(fragmentTemplate);

                // Deactivate the original object
                this.gameObject.SetActive(false);

                // Fire the completion callback
                if ((this.currentRefractureCount == 0) ||
                    (this.currentRefractureCount > 0 && this.refractureOptions.invokeCallbacks))
                {
                    if (callbackOptions.onCompleted != null)
                    {
                        callbackOptions.onCompleted.Invoke();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Creates a template object which each fragment will derive from
    /// </summary>
    /// <param name="preFracture">True if this object is being pre-fractured. This will freeze all of the fragments.</param>
    /// <returns></returns>
    private GameObject CreateFragmentTemplate()
    {
        // If pre-fracturing, make the fragments children of this object so they can easily be unfrozen later.
        // Otherwise, parent to this object's parent
        GameObject obj = new GameObject();
        obj.name = "Fragment";
        obj.tag = this.tag;

        obj.layer = LayerMask.NameToLayer("Ignore");

        // Update mesh to the new sliced mesh
        obj.AddComponent<MeshFilter>();

        // Add materials. Normal material goes in slot 1, cut material in slot 2
        var meshRenderer = obj.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterials = new Material[2] {
            this.GetComponent<MeshRenderer>().sharedMaterial,
            this.fractureOptions.insideMaterial
        };

        // Copy collider properties to fragment
        var thisCollider = this.GetComponent<Collider>();
        var fragmentCollider = obj.AddComponent<MeshCollider>();
        fragmentCollider.convex = true;
        fragmentCollider.sharedMaterial = thisCollider.sharedMaterial;
        fragmentCollider.isTrigger = thisCollider.isTrigger;

        // Copy rigid body properties to fragment
        var thisRigidBody = this.GetComponent<Rigidbody>();
        var fragmentRigidBody = obj.AddComponent<Rigidbody>();
        fragmentRigidBody.velocity = thisRigidBody.velocity;
        fragmentRigidBody.angularVelocity = thisRigidBody.angularVelocity;
        fragmentRigidBody.drag = thisRigidBody.drag;
        fragmentRigidBody.angularDrag = thisRigidBody.angularDrag;
        fragmentRigidBody.useGravity = thisRigidBody.useGravity;

        // If refracturing is enabled, create a copy of this component and add it to the template fragment object
        if (refractureOptions.enableRefracturing &&
           (this.currentRefractureCount < refractureOptions.maxRefractureCount))
        {
            CopyFractureComponent(obj);
        }

        return obj;
    }

    private void CopyFractureComponent(GameObject obj)
    {
        var fractureComponent = obj.AddComponent<FractureObject>();

        fractureComponent.fractureOptions = this.fractureOptions;
        fractureComponent.refractureOptions = this.refractureOptions;
        fractureComponent.callbackOptions = this.callbackOptions;
        fractureComponent.currentRefractureCount = this.currentRefractureCount + 1;
        fractureComponent.fragmentRoot = this.fragmentRoot;
    }
}
