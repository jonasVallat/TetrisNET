# Readme

Pour que l'application WPF puisse faire des requêtes au serveur il faut lancer le server IIS Express pour l'application TetrisAPI.

Il faudra également changé la constante `URI` de la classe `SingleHttpClientInstance` du projet Tetris pour qu'elle corresponde à l'uri de votre serveur IIS.