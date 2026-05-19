# Documentation Technique - Gestion de la Persistance (EF Core)

Ce document détaille les étapes réalisées pour la configuration de la base de données, la mise en place de l'ORM Entity Framework Core et la préparation des opérations de données.

## 1. Configuration de la Base de Données
L'application utilise **PostgreSQL** comme système de gestion de base de données.

- **Injection de dépendances** : Dans le fichier `Program.cs`, le service `ApplicationDbContext` a été enregistré pour utiliser le fournisseur Npgsql.
- **Chaîne de connexion** : Elle est récupérée depuis le fichier de configuration `appsettings.json` via la clé `DefaultConnection`.

## 2. Création du DbContext (`ApplicationDbContext`)
Le `ApplicationDbContext` centralise la configuration de la base de données et les règles métier liées aux données.

### Tables (DbSets)
Toutes les entités principales ont été mappées : `Produits`, `Categories`, `Fournisseurs`, `Clients`, `Commandes`, `Livraisons`, `Factures`, ainsi que les tables de détails (`CommandeDetails`, etc.).

### Configuration Fluent API (`OnModelCreating`)
- **Clés composites** : Configuration des clés primaires pour les tables de jointure (ex: `CommandeId` + `ProduitId`).
- **Sécurité & Soft Delete** : Mise en place de filtres globaux (`HasQueryFilter`) pour exclure automatiquement les données marquées comme supprimées (`IsDeleted`).
- **Intégrité référentielle** : Désactivation de la suppression en cascade au profit de `DeleteBehavior.Restrict` pour éviter les pertes de données accidentelles.

## 3. Architecture des Entités
- **BaseEntity** : Une classe abstraite partagée par toutes les entités pour assurer la traçabilité.
- **Audit Trail** : Gestion automatique des champs `DateCreation` et `DateModification`.
- **Soft Delete** : Utilisation de l'interface `ISoftDelete` pour transformer les suppressions physiques en archivage logique.

## 4. Automatisation des Opérations (Interception)
Les méthodes `SaveChanges` et `SaveChangesAsync` ont été surchargées pour inclure une logique d'interception automatique :
1. **Ajout** : Initialise la date de création.
2. **Modification** : Met à jour la date de modification.
3. **Suppression** : Intercepte l'état `Deleted`, change l'état en `Modified`, et bascule le flag `IsDeleted` à `true`.

## 5. Migrations Entity Framework
Pour appliquer ces changements à la base de données, les commandes suivantes sont utilisées :

```powershell
# Création d'une nouvelle migration
dotnet ef migrations add InitialCreate

# Mise à jour de la base de données
dotnet ef database update

# Changer le motde passe dans AppSettings.json par votre propre mot de passe.

```
# NB: Changer le motde passe dans AppSettings.json par votre propre mot de passe.

## 6. Préparation des Opérations de Base (CRUD)
L'infrastructure est prête pour le développement des contrôleurs :
- Les relations (1-N et N-N) sont configurées.
- Le chargement lié (Eager Loading) est facilité par les propriétés `virtual`.
- La validation des données est assurée par les `DataAnnotations` dans les modèles (ex: `[Required]`, `[EmailAddress]`).



---
*Projet développé dans le cadre du cours Framework ASP.NET.*