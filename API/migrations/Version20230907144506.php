<?php

declare(strict_types=1);

namespace DoctrineMigrations;

use Doctrine\DBAL\Schema\Schema;
use Doctrine\Migrations\AbstractMigration;

/**
 * Auto-generated Migration: Please modify to your needs!
 */
final class Version20230907144506 extends AbstractMigration
{
    public function getDescription(): string
    {
        return '';
    }

    public function up(Schema $schema): void
    {
        // this up() migration is auto-generated, please modify it to your needs
        $this->addSql('CREATE SEQUENCE disease_id_seq INCREMENT BY 1 MINVALUE 1 START 1');
        $this->addSql('CREATE SEQUENCE doctor_id_seq INCREMENT BY 1 MINVALUE 1 START 1');
        $this->addSql('CREATE SEQUENCE specialization_id_seq INCREMENT BY 1 MINVALUE 1 START 1');
        $this->addSql('CREATE TABLE disease (id INT NOT NULL, name VARCHAR(255) NOT NULL, PRIMARY KEY(id))');
        $this->addSql('CREATE TABLE disease_doctor (disease_id INT NOT NULL, doctor_id INT NOT NULL, PRIMARY KEY(disease_id, doctor_id))');
        $this->addSql('CREATE INDEX IDX_AB64352BD8355341 ON disease_doctor (disease_id)');
        $this->addSql('CREATE INDEX IDX_AB64352B87F4FB17 ON disease_doctor (doctor_id)');
        $this->addSql('CREATE TABLE doctor (id INT NOT NULL, first_name VARCHAR(50) NOT NULL, last_name VARCHAR(100) NOT NULL, phone VARCHAR(12) DEFAULT NULL, description TEXT DEFAULT NULL, education TEXT DEFAULT NULL, PRIMARY KEY(id))');
        $this->addSql('CREATE TABLE doctor_specialization (doctor_id INT NOT NULL, specialization_id INT NOT NULL, PRIMARY KEY(doctor_id, specialization_id))');
        $this->addSql('CREATE INDEX IDX_1187285D87F4FB17 ON doctor_specialization (doctor_id)');
        $this->addSql('CREATE INDEX IDX_1187285DFA846217 ON doctor_specialization (specialization_id)');
        $this->addSql('CREATE TABLE specialization (id INT NOT NULL, name VARCHAR(100) NOT NULL, description TEXT DEFAULT NULL, PRIMARY KEY(id))');
        $this->addSql('ALTER TABLE disease_doctor ADD CONSTRAINT FK_AB64352BD8355341 FOREIGN KEY (disease_id) REFERENCES disease (id) ON DELETE CASCADE NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE disease_doctor ADD CONSTRAINT FK_AB64352B87F4FB17 FOREIGN KEY (doctor_id) REFERENCES doctor (id) ON DELETE CASCADE NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE doctor_specialization ADD CONSTRAINT FK_1187285D87F4FB17 FOREIGN KEY (doctor_id) REFERENCES doctor (id) ON DELETE CASCADE NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE doctor_specialization ADD CONSTRAINT FK_1187285DFA846217 FOREIGN KEY (specialization_id) REFERENCES specialization (id) ON DELETE CASCADE NOT DEFERRABLE INITIALLY IMMEDIATE');
    }

    public function down(Schema $schema): void
    {
        // this down() migration is auto-generated, please modify it to your needs
        $this->addSql('CREATE SCHEMA public');
        $this->addSql('DROP SEQUENCE disease_id_seq CASCADE');
        $this->addSql('DROP SEQUENCE doctor_id_seq CASCADE');
        $this->addSql('DROP SEQUENCE specialization_id_seq CASCADE');
        $this->addSql('ALTER TABLE disease_doctor DROP CONSTRAINT FK_AB64352BD8355341');
        $this->addSql('ALTER TABLE disease_doctor DROP CONSTRAINT FK_AB64352B87F4FB17');
        $this->addSql('ALTER TABLE doctor_specialization DROP CONSTRAINT FK_1187285D87F4FB17');
        $this->addSql('ALTER TABLE doctor_specialization DROP CONSTRAINT FK_1187285DFA846217');
        $this->addSql('DROP TABLE disease');
        $this->addSql('DROP TABLE disease_doctor');
        $this->addSql('DROP TABLE doctor');
        $this->addSql('DROP TABLE doctor_specialization');
        $this->addSql('DROP TABLE specialization');
    }
}
