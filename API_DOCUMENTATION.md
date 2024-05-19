# NELSON CUERVO TEST - DOCUMENTATION API

## ROUTER ET ENDPOINTS:

### Départs

#### 01. GET ALL

- Route : [host]/api/arrivees
- Méthode : GET
- Paramètres : Aucun
- Réponse :
  - Code 200 (OK) : Retourne une liste de toutes les arrivées, organisées par ID.
  - Code 404 (Not Found) : En cas d'erreur.
  - Code 403 (Forbidden) : En cas d'accès non autorisé.

#### 02. GET BY ID

- Route : [host]/api/arrivees/{arriveeId:int}
  Exemple : [host]/api/arrivees/2
- Méthode : GET
- Paramètres :
  - arriveeId : l'ID de l'arrivée à rechercher (de type int)
- Réponse :
  - Code 200 (OK) : Retourne les détails de l'arrivée correspondante.
  - Code 404 (Not Found) : Si l'arrivée avec l'ID spécifié n'est pas trouvée.
  - Code 403 (Forbidden) : En cas d'accès non autorisé.
  - Code 400 (Bad Request) : Si la requête est mal formulée.

#### 03. CREATE

- Route : [host]/api/arrivees
- Méthode : POST
- Paramètres dans le corps de la requête :
  - nomNavire : string
  - portOrigine : string
  - terminal : string
  - typeCargaison : string
- Réponse :
  - Code 201 (Created) : Retourne les détails de l'arrivée créée en format JSON.
  - Code 400 (Bad Request) : Si la requête est mal formulée ou si le modèle est invalide.
  - Code 500 (Internal Server Error) : Si un problème se produit lors de la création de l'arrivée.

#### 04. UPDATE

- Route : [host]/api/arrivees/{arriveeId:int}
- Méthode : PATCH
- Paramètres dans le corps de la requête :
  - id : int (doit être fourni pour identifier l'enregistrement à modifier)
  - nomNavire : string
  - portOrigine : string
  - terminal : string
  - typeCargaison : string
- Réponse :
  - Code 204 (No Content) : Indique que la mise à jour a été effectuée avec succès.
  - Code 400 (Bad Request) : Si la requête est mal formulée ou si le modèle est invalide.
  - Code 404 (Not Found) : Si l'enregistrement avec l'ID spécifié n'est pas trouvé.
  - Code 500 (Internal Server Error) : Si un problème se produit lors de la mise à jour de l'arrivée.

#### 05. DELETE

- Route : [host]/api/arrivees/{arriveId:int}
- Méthode : DELETE
- Paramètre dans l'URL :
  - arriveId : int (ID de l'arrivée à supprimer)
- Réponse :
  - Code 204 (No Content) : Indique que la suppression a été effectuée avec succès.
  - Code 400 (Bad Request) : Si la requête est mal formulée ou si le modèle est invalide.
  - Code 403 (Forbidden) : Si l'utilisateur n'est pas autorisé à effectuer cette action.
  - Code 404 (Not Found) : Si l'enregistrement avec l'ID spécifié n'est pas trouvé.
  - Code 500 (Internal Server Error) : Si un problème se produit lors de la suppression de l'arrivée.

### Départs

#### 01. GET ALL

- Route : [host]/api/departs
- Méthode : GET
- Paramètres : Aucun
- Réponse :
  - Code 200 (OK) : Indique que l'opération a réussi et renvoie tous les enregistrements de départs trouvés dans la base de données, ordonnés par ID.
  - Code 403 (Forbidden) : Si l'utilisateur n'est pas autorisé à effectuer cette action.

#### 02. GET BY ID

- Route : [host]/api/departs/{departId}
- Méthode : GET
- Paramètre :
  - departId (int) : L'ID de l'enregistrement de départ à rechercher
- Réponse :
  - Code 200 (OK) : Indique que l'opération a réussi et renvoie les informations de l'enregistrement de départ.
  - Code 400 (Bad Request) : Si la requête est invalide ou si l'ID fourni est incorrect.
  - Code 404 (Not Found) : Si aucun enregistrement avec l'ID spécifié n'est trouvé.
  - Code 403 (Forbidden) : Si l'utilisateur n'est pas autorisé à effectuer cette action.

#### 03. CREATE

- Route : [host]/api/departs
- Méthode : POST
- Corps de la requête :
  - createDepartDto (CreateDepartsDto) : Objet contenant les informations du nouveau départ à créer
    - id (int) : L'ID du départ (auto-incrémenté dans la base de données, ne doit pas être renseigné)
    - nomNavire (string) : Nom du navire (sera récupéré à partir de l'arrivée associée)
    - portDestination (string) : Port de destination du navire
    - quai (string) : Quai de départ du navire
    - arriveeId (int) : ID de l'arrivée associée au départ à créer
- Réponse :
  - Code 201 (Created) : Indique que le départ a été créé avec succès et renvoie le départ créé avec son ID.
  - Code 400 (Bad Request) : Si le modèle de données reçu est invalide ou si l'objet est null.
  - Code 404 (Not Found) : Si l'arrivée associée au départ n'est pas trouvée dans la base de données.
  - Code 500 (Internal Server Error) : Si une erreur survient lors de la création du départ dans la base de données.

#### 04. UPDATE

- Route : [host]/api/departs/{departId}
- Méthode : PATCH
- Paramètre
