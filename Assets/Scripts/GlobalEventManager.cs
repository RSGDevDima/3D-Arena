using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public static class GlobalEventManager
{
    public static class OnEnemyDamage
    {
        static Action s_listenerList;

        public static void AddListener(Action listener){
            s_listenerList += listener;
        }

        public static void RemoveListener(Action listener){
            s_listenerList -= listener;
        }

        public static void Fire(){
            s_listenerList?.Invoke();
        }
    }
    
    public static class OnRewardedEnemyDeath
    {
        static Action<Enemy, float> s_listenerList;

        public static void AddListener(Action<Enemy, float> listener){
            s_listenerList += listener;
        }

        public static void RemoveListener(Action<Enemy, float> listener){
            s_listenerList -= listener;
        }

        public static void Fire(Enemy enemy, float strengthAmount){
            OnEnemyDeath.Fire(enemy);
            s_listenerList?.Invoke(enemy, strengthAmount);
        }
    }
    
    public static class OnEnemyDeath
    {
        static Action<Enemy> s_listenerList;

        public static void AddListener(Action<Enemy> listener)
        {
            s_listenerList += listener;
        }

        public static void RemoveListener(Action<Enemy> listener)
        {
            s_listenerList -= listener;
        }

        public static void Fire(Enemy enemy)
        {
            s_listenerList?.Invoke(enemy);
        }
    }

    public static class OnExtraDeath{
        static Action s_listenerList;

        public static void AddListener(Action listener){
            s_listenerList += listener;
        }

        public static void RemoveListener(Action listener){
            s_listenerList -= listener;
        }

        public static void Fire(){
            s_listenerList?.Invoke();
        }
    }

    public static class OnOppositeCornerMove
    {
        static Action s_listenerList;

        public static void AddListener(Action listener)
        {
            s_listenerList += listener;
        }

        public static void RemoveListener(Action listener)
        {
            s_listenerList -= listener;
        }

        public static void Fire()
        {
            s_listenerList?.Invoke();
        }
    }

    public static class OnHealthChange
    {
        static Action<float, float> s_listenerList;

        public static void AddListener(Action<float, float> listener)
        {
            s_listenerList += listener;
        }

        public static void RemoveListener(Action<float, float> listener)
        {
            s_listenerList -= listener;
        }

        public static void Fire(float currentHealth, float maxHealth)
        {
            s_listenerList?.Invoke(currentHealth, maxHealth);
        }
    }

    public static class OnPlayerInit
    {
        static Action<float, float, float, float> s_listenerList;

        public static void AddListener(Action<float, float, float, float> listener)
        {
            s_listenerList += listener;
        }

        public static void RemoveListener(Action<float, float, float, float> listener)
        {
            s_listenerList -= listener;
        }

        public static void Fire(float currentHealth, float maxHealth, float currentStrength, float maxStrength)
        {
            s_listenerList?.Invoke(currentHealth, maxHealth, currentStrength, maxStrength);
        }
    }



    public static class OnStrenghtChange
    {
        static Action<float, float> s_listenerList;

        public static void AddListener(Action<float, float> listener)
        {
            s_listenerList += listener;
        }

        public static void RemoveListener(Action<float, float> listener)
        {
            s_listenerList -= listener;
        }

        public static void Fire(float currentStrength, float maxStrength)
        {
            s_listenerList?.Invoke(currentStrength, maxStrength);
        }
    }

    public static class OnScoreChange
    {
        static Action<int> s_listenerList;

        public static void AddListener(Action<int> listener)
        {
            s_listenerList += listener;
        }

        public static void RemoveListener(Action<int> listener)
        {
            s_listenerList -= listener;
        }

        public static void Fire(int score)
        {
            s_listenerList?.Invoke(score);
        }
    }

    public static class OnScoreInit
    {
        static Action<int> s_listenerList;

        public static void AddListener(Action<int> listener)
        {
            s_listenerList += listener;
        }

        public static void RemoveListener(Action<int> listener)
        {
            s_listenerList -= listener;
        }

        public static void Fire(int score)
        {
            s_listenerList?.Invoke(score);
        }
    }

    public static class OnEndgame
    {
        static Action s_listenerList;

        public static void AddListener(Action listener)
        {
            s_listenerList += listener;
        }

        public static void RemoveListener(Action listener)
        {
            s_listenerList -= listener;
        }

        public static void Fire()
        {
            s_listenerList?.Invoke();
        }
    }

    public static class OnUltraStateChanged
    {
        static Action<bool> s_listenerList;

        public static void AddListener(Action<bool> listener)
        {
            s_listenerList += listener;
        }

        public static void RemoveListener(Action<bool> listener)
        {
            s_listenerList -= listener;
        }

        public static void Fire(bool ultraState)
        {
            s_listenerList?.Invoke(ultraState);
        }
    }
}
