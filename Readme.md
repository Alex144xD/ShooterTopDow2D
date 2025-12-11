README 
Descripción del Proyecto
Este juego en Unity es un topDown incluye disparos, enemigos con IA, salud del jugador y música de fondo.
Patrones de Diseño Implementados
1. Singleton – MusicManager
Archivo: MusicManager.cs
•	Asegura que solo exista una instancia del manejador de música.
•	Mantiene la música entre escenas.
•	Evita creación de duplicados.
2. Object Pool – BulletPool + Bullet
Archivos: BulletPool.cs, Bullet.cs
•	Las balas no se destruyen; se reutilizan.
•	Se usa una cola de objetos para reactivar balas.
•	Optimiza rendimiento al evitar Instantiate/Destroy.
3. Factory Method – EnemyFactory + IFactory + EnemySpawner
Archivos: IFactory.cs, EnemyFactory.cs, EnemySpawner.cs
•	EnemySpawner depende solo de la interfaz.
•	EnemyFactory decide qué prefab instanciar.
•	Permite cambiar el tipo de enemigo sin tocar el spawner.
4. MVP (Model–View–Presenter) – Sistema de Salud
Archivos: PlayerHealthModel.cs, PlayerHealthView.cs, PlayerHealthPresenter.cs
•	Model maneja salud y daño.
•	View solo muestra el slider.
•	Presenter conecta model y vista.
5. State Machine (FSM) – EnemyAI
Archivo: EnemyAI.cs
Estados:
•	Patrol
•	Chase
•	Attack
•	Dead
Cada estado tiene comportamiento propio. Las transiciones se manejan en ChangeState().
Resumen de Patrones que Sí Cuentas para tu Examen
•	Singleton
•	Object Pool
•	Factory Method
•	State Machine
•	MVP (arquitectura adicional, no patrón clásico)
