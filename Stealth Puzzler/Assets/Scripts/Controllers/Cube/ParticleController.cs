using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
   [SerializeField] private List<ParticleSystem> _dropParticles;
   [SerializeField] private List<ParticleSystem> _impactParticles;
   private Quaternion _defaultRotation;

   private void Start()
   {
      _defaultRotation = _dropParticles[0].transform.rotation;
   }

   public void PlayDropVFX(Vector3 referencePosition)
   {
      
      foreach (var dropParticle in _dropParticles)
      {
         dropParticle.transform.position = referencePosition + Vector3.down;
         dropParticle.transform.rotation = _defaultRotation;
         dropParticle.Play();
      }
   }

   public void PlayDropImpactVFX(Vector3 referencePosition)
   {
      foreach (var impactParticle in _impactParticles)
      {
         impactParticle.transform.position = referencePosition + Vector3.down;
         impactParticle.transform.rotation = _defaultRotation;
         impactParticle.Play();
      }
   }

   public void StopDropVFX()
   {
      foreach (var dropParticle in _dropParticles)
      {
         dropParticle.Stop();
      }
   }

   public void StopDropImpactVFX()
   {
      foreach (var impactParticle in _impactParticles)
      {
         impactParticle.Stop();
      }
   }
}
