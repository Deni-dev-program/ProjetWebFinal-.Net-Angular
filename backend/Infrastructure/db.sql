-- ================================================
-- Script de création de la base de données
-- Application : Clinique Médicale
-- ================================================

CREATE DATABASE IF NOT EXISTS clinique_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE clinique_db;

-- ================================================
-- Table : patient
-- ================================================
CREATE TABLE IF NOT EXISTS patient (
    idPatient   INT(11)      NOT NULL AUTO_INCREMENT,
    nom         VARCHAR(50)  NOT NULL,
    prenom      VARCHAR(50)  NOT NULL,
    dateNaissance DATE        NOT NULL,
    sexe        CHAR(1)      NOT NULL,
    telephone   VARCHAR(15)  NOT NULL,
    email       VARCHAR(100) NOT NULL,
    PRIMARY KEY (idPatient)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- Table : medecin
-- ================================================
CREATE TABLE IF NOT EXISTS medecin (
    idMedecin   INT(11)      NOT NULL AUTO_INCREMENT,
    nom         VARCHAR(50)  NOT NULL,
    specialite  VARCHAR(50)  NOT NULL,
    emailPro    VARCHAR(100) NOT NULL,
    PRIMARY KEY (idMedecin)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- Table : dossiermedical
-- ================================================
CREATE TABLE IF NOT EXISTS dossiermedical (
    idDossier     INT(11)      NOT NULL AUTO_INCREMENT,
    groupeSanguin VARCHAR(5)   NOT NULL,
    antecedents   TEXT,
    allergies     TEXT,
    idPatient     INT(11)      NOT NULL,
    PRIMARY KEY (idDossier),
    UNIQUE KEY uq_dossier_patient (idPatient),
    FOREIGN KEY (idPatient) REFERENCES patient(idPatient) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- Table : consultation
-- ================================================
CREATE TABLE IF NOT EXISTS consultation (
    idConsultation  INT(11)        NOT NULL AUTO_INCREMENT,
    dateConsultation DATETIME      NOT NULL,
    diagnostic      TEXT           NOT NULL,
    prix            DECIMAL(10,2)  NOT NULL,
    idDossier       INT(11)        NOT NULL,
    idMedecin       INT(11)        NOT NULL,
    PRIMARY KEY (idConsultation),
    FOREIGN KEY (idDossier)  REFERENCES dossiermedical(idDossier) ON DELETE CASCADE,
    FOREIGN KEY (idMedecin)  REFERENCES medecin(idMedecin)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- Table : facture
-- ================================================
CREATE TABLE IF NOT EXISTS facture (
    idFacture      INT(11)        NOT NULL AUTO_INCREMENT,
    dateFacture    DATE           NOT NULL,
    montantTotal   DECIMAL(10,2)  NOT NULL,
    statutPaiement VARCHAR(20)    NOT NULL DEFAULT 'en attente',
    idConsultation INT(11)        NOT NULL,
    PRIMARY KEY (idFacture),
    UNIQUE KEY uq_facture_consultation (idConsultation),
    FOREIGN KEY (idConsultation) REFERENCES consultation(idConsultation) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- Table : prescription
-- ================================================
CREATE TABLE IF NOT EXISTS prescription (
    idPrescription   INT(11) NOT NULL AUTO_INCREMENT,
    datePrescription DATE    NOT NULL,
    idConsultation   INT(11) NOT NULL,
    PRIMARY KEY (idPrescription),
    FOREIGN KEY (idConsultation) REFERENCES consultation(idConsultation) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- Table : medicament
-- ================================================
CREATE TABLE IF NOT EXISTS medicament (
    idMedicament   INT(11)       NOT NULL AUTO_INCREMENT,
    nomCommercial  VARCHAR(100)  NOT NULL,
    principeActif  VARCHAR(100)  NOT NULL,
    dosage         VARCHAR(50)   NOT NULL,
    PRIMARY KEY (idMedicament)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- Table : ligneprescription
-- ================================================
CREATE TABLE IF NOT EXISTS ligneprescription (
    idLigne        INT(11)       NOT NULL AUTO_INCREMENT,
    quantite       INT(11)       NOT NULL,
    posologie      VARCHAR(255)  NOT NULL,
    idPrescription INT(11)       NOT NULL,
    idMedicament   INT(11)       NOT NULL,
    PRIMARY KEY (idLigne),
    FOREIGN KEY (idPrescription) REFERENCES prescription(idPrescription) ON DELETE CASCADE,
    FOREIGN KEY (idMedicament)   REFERENCES medicament(idMedicament)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- Table : rendezvous
-- ================================================
CREATE TABLE IF NOT EXISTS rendezvous (
    idRDV     INT(11)       NOT NULL AUTO_INCREMENT,
    dateHeure DATETIME      NOT NULL,
    motif     VARCHAR(255)  NOT NULL,
    statut    VARCHAR(20)   NOT NULL DEFAULT 'planifié',
    idPatient INT(11)       NOT NULL,
    idMedecin INT(11)       NOT NULL,
    PRIMARY KEY (idRDV),
    FOREIGN KEY (idPatient) REFERENCES patient(idPatient) ON DELETE CASCADE,
    FOREIGN KEY (idMedecin) REFERENCES medecin(idMedecin)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- Table : secretaire
-- ================================================
CREATE TABLE IF NOT EXISTS secretaire (
    idSecretaire INT(11)      NOT NULL AUTO_INCREMENT,
    nom          VARCHAR(50)  NOT NULL,
    prenom       VARCHAR(50)  NOT NULL,
    email        VARCHAR(100) NOT NULL UNIQUE,
    PRIMARY KEY (idSecretaire)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- Table : utilisateur (authentification)
-- ================================================
CREATE TABLE IF NOT EXISTS utilisateur (
    idUtilisateur INT(11)       NOT NULL AUTO_INCREMENT,
    email         VARCHAR(100)  NOT NULL UNIQUE,
    passwordHash  VARCHAR(255)  NOT NULL,
    role          VARCHAR(20)   NOT NULL,  -- 'patient', 'medecin', 'secretaire'
    idRef         INT(11)       NOT NULL,
    PRIMARY KEY (idUtilisateur)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- Données de démonstration
-- ================================================

INSERT IGNORE INTO medecin (idMedecin, nom, specialite, emailPro) VALUES
(1, 'Martin',   'Cardiologie',      'martin@clinique.be'),
(2, 'Dubois',   'Médecine générale','dubois@clinique.be'),
(3, 'Lambert',  'Pédiatrie',        'lambert@clinique.be');

INSERT IGNORE INTO patient (idPatient, nom, prenom, dateNaissance, sexe, telephone, email) VALUES
(1, 'Dupont',   'Alice',  '1985-03-12', 'F', '0470123456', 'alice.dupont@mail.be'),
(2, 'Leroy',    'Marc',   '1972-07-28', 'M', '0479654321', 'marc.leroy@mail.be'),
(3, 'Bernard',  'Sophie', '1995-11-05', 'F', '0468987654', 'sophie.bernard@mail.be');

INSERT IGNORE INTO dossiermedical (idDossier, groupeSanguin, antecedents, allergies, idPatient) VALUES
(1, 'A+',  'Hypertension',         'Pénicilline',  1),
(2, 'O-',  'Diabète type 2',       'Aucune',        2),
(3, 'B+',  'Asthme',               'Aspirine',      3);

INSERT IGNORE INTO medicament (idMedicament, nomCommercial, principeActif, dosage) VALUES
(1, 'Aspirine',    'Acide acétylsalicylique', '500mg'),
(2, 'Amoxicilline','Amoxicilline',             '1g'),
(3, 'Doliprane',   'Paracétamol',             '1000mg'),
(4, 'Ventoline',   'Salbutamol',              '100µg');

INSERT IGNORE INTO consultation (idConsultation, dateConsultation, diagnostic, prix, idDossier, idMedecin) VALUES
(1, '2026-05-10 09:00:00', 'Hypertension artérielle contrôlée', 50.00, 1, 1),
(2, '2026-05-15 14:30:00', 'Bronchite aiguë',                   45.00, 3, 2);

INSERT IGNORE INTO facture (idFacture, dateFacture, montantTotal, statutPaiement, idConsultation) VALUES
(1, '2026-05-10', 50.00, 'payé',        1),
(2, '2026-05-15', 45.00, 'en attente',  2);

INSERT IGNORE INTO rendezvous (idRDV, dateHeure, motif, statut, idPatient, idMedecin) VALUES
(1, '2026-06-20 10:00:00', 'Contrôle tension',   'planifié',  1, 1),
(2, '2026-06-21 11:30:00', 'Consultation annuelle','planifié', 2, 2),
(3, '2026-06-22 09:00:00', 'Suivi asthme',        'planifié', 3, 3);
