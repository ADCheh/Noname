﻿using System;
using Infrastructure.Factory;
using Infrastructure.Services;
using UnityEngine;

namespace Enemy
{
    public class RotateToHero : Follow
    {
        public float Speed;

        private Transform _heroTransform;
        private IGameFactory _gameFactory;
        private Vector3 _positionToLook;

        private void Start()
        {
            _gameFactory = AllServices.Containter.Single<IGameFactory>();

            if (HeroExists())
                InitializeHeroTransform();
            else
                _gameFactory.HeroCreated += InitializeHeroTransform;
        }

        private void Update()
        {
            if (Initialized())
                RotateTowardsHero();
        }

        private bool HeroExists()
        {
            return _gameFactory.HeroGameObject != null;
        }

        private void InitializeHeroTransform()
        {
            _heroTransform = _gameFactory.HeroGameObject.transform;
        }

        private void RotateTowardsHero()
        {
            UpdatePositionToLookAt();

            transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
        }

        private void UpdatePositionToLookAt()
        {
            Vector3 positionDiff = _heroTransform.position - transform.position;
            _positionToLook = new Vector3(positionDiff.x, transform.position.y, positionDiff.z);
        }

        private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook)
        {
            return Quaternion.Lerp(rotation, TargetRotation(positionToLook), SpeedFactor());
        }

        private Quaternion TargetRotation(Vector3 position)
        {
            return Quaternion.LookRotation(position);
        }

        private float SpeedFactor()
        {
            return Speed * Time.deltaTime;
        }

        private bool Initialized()
        {
            return _heroTransform != null;
        }
    }
}