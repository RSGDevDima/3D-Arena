using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public static class GlobalEventManager
{
    public static class OnRewardedEnemyDeath
    {
        static Action<Enemy, float> listenerList;

        public static void AddListener(Action<Enemy, float> listener){
            listenerList += listener;
        }

        public static void RemoveListener(Action<Enemy, float> listener){
            listenerList -= listener;
        }

        public static void Fire(Enemy enemy, float strengthAmount){
            OnEnemyDeath.Fire(enemy);
            listenerList?.Invoke(enemy, strengthAmount);
        }
    }
    
    public static class OnEnemyDeath
    {
        static Action<Enemy> listenerList;

        public static void AddListener(Action<Enemy> listener)
        {
            listenerList += listener;
        }

        public static void RemoveListener(Action<Enemy> listener)
        {
            listenerList -= listener;
        }

        public static void Fire(Enemy enemy)
        {
            listenerList?.Invoke(enemy);
        }
    }

    public static class OnExtraDeath{
        static Action listenerList;

        public static void AddListener(Action listener){
            listenerList += listener;
        }

        public static void RemoveListener(Action listener){
            listenerList -= listener;
        }

        public static void Fire(){
            listenerList?.Invoke();
        }
    }

    public static class OnOppositeCornerMove
    {
        static Action listenerList;

        public static void AddListener(Action listener)
        {
            listenerList += listener;
        }

        public static void RemoveListener(Action listener)
        {
            listenerList -= listener;
        }

        public static void Fire()
        {
            listenerList?.Invoke();
        }
    }

    public static class OnHealthChange
    {
        static Action<float, float> listenerList;

        public static void AddListener(Action<float, float> listener)
        {
            listenerList += listener;
        }

        public static void RemoveListener(Action<float, float> listener)
        {
            listenerList -= listener;
        }

        public static void Fire(float currentHealth, float maxHealth)
        {
            listenerList?.Invoke(currentHealth, maxHealth);
        }
    }

    public static class OnPlayerInit
    {
        static Action<float, float, float, float> listenerList;

        public static void AddListener(Action<float, float, float, float> listener)
        {
            listenerList += listener;
        }

        public static void RemoveListener(Action<float, float, float, float> listener)
        {
            listenerList -= listener;
        }

        public static void Fire(float currentHealth, float maxHealth, float currentStrength, float maxStrength)
        {
            listenerList?.Invoke(currentHealth, maxHealth, currentStrength, maxStrength);
        }
    }



    public static class OnStrenghtChange
    {
        static Action<float, float> listenerList;

        public static void AddListener(Action<float, float> listener)
        {
            listenerList += listener;
        }

        public static void RemoveListener(Action<float, float> listener)
        {
            listenerList -= listener;
        }

        public static void Fire(float currentStrength, float maxStrength)
        {
            listenerList?.Invoke(currentStrength, maxStrength);
        }
    }

    public static class OnScoreChange
    {
        static Action<int> listenerList;

        public static void AddListener(Action<int> listener)
        {
            listenerList += listener;
        }

        public static void RemoveListener(Action<int> listener)
        {
            listenerList -= listener;
        }

        public static void Fire(int score)
        {
            listenerList?.Invoke(score);
        }
    }

    public static class OnScoreInit
    {
        static Action<int> listenerList;

        public static void AddListener(Action<int> listener)
        {
            listenerList += listener;
        }

        public static void RemoveListener(Action<int> listener)
        {
            listenerList -= listener;
        }

        public static void Fire(int score)
        {
            listenerList?.Invoke(score);
        }
    }

    public static class OnEndgame
    {
        static Action listenerList;

        public static void AddListener(Action listener)
        {
            listenerList += listener;
        }

        public static void RemoveListener(Action listener)
        {
            listenerList -= listener;
        }

        public static void Fire()
        {
            listenerList?.Invoke();
        }
    }

    public static class OnUltraStateChanged
    {
        static Action<bool> listenerList;

        public static void AddListener(Action<bool> listener)
        {
            listenerList += listener;
        }

        public static void RemoveListener(Action<bool> listener)
        {
            listenerList -= listener;
        }

        public static void Fire(bool ultraState)
        {
            listenerList?.Invoke(ultraState);
        }
    }
}
