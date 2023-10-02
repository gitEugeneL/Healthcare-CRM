<?php

declare(strict_types=1);

namespace DoctrineMigrations;

use Doctrine\DBAL\Schema\Schema;
use Doctrine\Migrations\AbstractMigration;

/**
 * Auto-generated Migration: Please modify to your needs!
 */
final class Version20231002161303 extends AbstractMigration
{
    public function getDescription(): string
    {
        return '';
    }

    public function up(Schema $schema): void
    {
        // this up() migration is auto-generated, please modify it to your needs
        $this->addSql('CREATE SEQUENCE address_id_seq INCREMENT BY 1 MINVALUE 1 START 1');
        $this->addSql('CREATE SEQUENCE disease_id_seq INCREMENT BY 1 MINVALUE 1 START 1');
        $this->addSql('CREATE SEQUENCE doctor_id_seq INCREMENT BY 1 MINVALUE 1 START 1');
        $this->addSql('CREATE SEQUENCE image_id_seq INCREMENT BY 1 MINVALUE 1 START 1');
        $this->addSql('CREATE SEQUENCE manager_id_seq INCREMENT BY 1 MINVALUE 1 START 1');
        $this->addSql('CREATE SEQUENCE patient_id_seq INCREMENT BY 1 MINVALUE 1 START 1');
        $this->addSql('CREATE SEQUENCE refresh_tokens_id_seq INCREMENT BY 1 MINVALUE 1 START 1');
        $this->addSql('CREATE SEQUENCE specialization_id_seq INCREMENT BY 1 MINVALUE 1 START 1');
        $this->addSql('CREATE SEQUENCE "user_id_seq" INCREMENT BY 1 MINVALUE 1 START 1');
        $this->addSql('CREATE TABLE address (id INT NOT NULL, city VARCHAR(100) DEFAULT NULL, street VARCHAR(100) DEFAULT NULL, house_number VARCHAR(10) DEFAULT NULL, apartment_number VARCHAR(10) DEFAULT NULL, postal_code VARCHAR(10) DEFAULT NULL, province VARCHAR(100) DEFAULT NULL, PRIMARY KEY(id))');
        $this->addSql('CREATE TABLE disease (id INT NOT NULL, name VARCHAR(50) NOT NULL, PRIMARY KEY(id))');
        $this->addSql('CREATE UNIQUE INDEX UNIQ_F3B6AC15E237E06 ON disease (name)');
        $this->addSql('CREATE TABLE doctor (id INT NOT NULL, user_id INT NOT NULL, status VARCHAR(255) NOT NULL, description TEXT DEFAULT NULL, education TEXT DEFAULT NULL, PRIMARY KEY(id))');
        $this->addSql('CREATE UNIQUE INDEX UNIQ_1FC0F36AA76ED395 ON doctor (user_id)');
        $this->addSql('CREATE TABLE doctor_specialization (doctor_id INT NOT NULL, specialization_id INT NOT NULL, PRIMARY KEY(doctor_id, specialization_id))');
        $this->addSql('CREATE INDEX IDX_1187285D87F4FB17 ON doctor_specialization (doctor_id)');
        $this->addSql('CREATE INDEX IDX_1187285DFA846217 ON doctor_specialization (specialization_id)');
        $this->addSql('CREATE TABLE doctor_disease (doctor_id INT NOT NULL, disease_id INT NOT NULL, PRIMARY KEY(doctor_id, disease_id))');
        $this->addSql('CREATE INDEX IDX_9C96096C87F4FB17 ON doctor_disease (doctor_id)');
        $this->addSql('CREATE INDEX IDX_9C96096CD8355341 ON doctor_disease (disease_id)');
        $this->addSql('CREATE TABLE image (id INT NOT NULL, user_id INT DEFAULT NULL, name VARCHAR(100) NOT NULL, type VARCHAR(20) NOT NULL, image_data BYTEA NOT NULL, PRIMARY KEY(id))');
        $this->addSql('CREATE UNIQUE INDEX UNIQ_C53D045FA76ED395 ON image (user_id)');
        $this->addSql('CREATE TABLE manager (id INT NOT NULL, user_id INT NOT NULL, position VARCHAR(150) DEFAULT NULL, PRIMARY KEY(id))');
        $this->addSql('CREATE UNIQUE INDEX UNIQ_FA2425B9A76ED395 ON manager (user_id)');
        $this->addSql('CREATE TABLE patient (id INT NOT NULL, address_id INT NOT NULL, user_id INT NOT NULL, pesel VARCHAR(11) DEFAULT NULL, date_of_birth DATE DEFAULT NULL, insurance VARCHAR(255) DEFAULT NULL, PRIMARY KEY(id))');
        $this->addSql('CREATE UNIQUE INDEX UNIQ_1ADAD7EBF5B7AF75 ON patient (address_id)');
        $this->addSql('CREATE UNIQUE INDEX UNIQ_1ADAD7EBA76ED395 ON patient (user_id)');
        $this->addSql('CREATE TABLE refresh_tokens (id INT NOT NULL, refresh_token VARCHAR(128) NOT NULL, username VARCHAR(255) NOT NULL, valid TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL, PRIMARY KEY(id))');
        $this->addSql('CREATE UNIQUE INDEX UNIQ_9BACE7E1C74F2195 ON refresh_tokens (refresh_token)');
        $this->addSql('CREATE TABLE specialization (id INT NOT NULL, name VARCHAR(100) NOT NULL, description TEXT DEFAULT NULL, PRIMARY KEY(id))');
        $this->addSql('CREATE TABLE "user" (id INT NOT NULL, email VARCHAR(150) NOT NULL, password VARCHAR(150) NOT NULL, first_name VARCHAR(50) NOT NULL, last_name VARCHAR(100) NOT NULL, phone VARCHAR(12) DEFAULT NULL, roles JSON NOT NULL, PRIMARY KEY(id))');
        $this->addSql('CREATE UNIQUE INDEX UNIQ_8D93D649E7927C74 ON "user" (email)');
        $this->addSql('ALTER TABLE doctor ADD CONSTRAINT FK_1FC0F36AA76ED395 FOREIGN KEY (user_id) REFERENCES "user" (id) NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE doctor_specialization ADD CONSTRAINT FK_1187285D87F4FB17 FOREIGN KEY (doctor_id) REFERENCES doctor (id) ON DELETE CASCADE NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE doctor_specialization ADD CONSTRAINT FK_1187285DFA846217 FOREIGN KEY (specialization_id) REFERENCES specialization (id) ON DELETE CASCADE NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE doctor_disease ADD CONSTRAINT FK_9C96096C87F4FB17 FOREIGN KEY (doctor_id) REFERENCES doctor (id) ON DELETE CASCADE NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE doctor_disease ADD CONSTRAINT FK_9C96096CD8355341 FOREIGN KEY (disease_id) REFERENCES disease (id) ON DELETE CASCADE NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE image ADD CONSTRAINT FK_C53D045FA76ED395 FOREIGN KEY (user_id) REFERENCES "user" (id) NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE manager ADD CONSTRAINT FK_FA2425B9A76ED395 FOREIGN KEY (user_id) REFERENCES "user" (id) NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE patient ADD CONSTRAINT FK_1ADAD7EBF5B7AF75 FOREIGN KEY (address_id) REFERENCES address (id) NOT DEFERRABLE INITIALLY IMMEDIATE');
        $this->addSql('ALTER TABLE patient ADD CONSTRAINT FK_1ADAD7EBA76ED395 FOREIGN KEY (user_id) REFERENCES "user" (id) NOT DEFERRABLE INITIALLY IMMEDIATE');
    }

    public function down(Schema $schema): void
    {
        // this down() migration is auto-generated, please modify it to your needs
        $this->addSql('CREATE SCHEMA public');
        $this->addSql('DROP SEQUENCE address_id_seq CASCADE');
        $this->addSql('DROP SEQUENCE disease_id_seq CASCADE');
        $this->addSql('DROP SEQUENCE doctor_id_seq CASCADE');
        $this->addSql('DROP SEQUENCE image_id_seq CASCADE');
        $this->addSql('DROP SEQUENCE manager_id_seq CASCADE');
        $this->addSql('DROP SEQUENCE patient_id_seq CASCADE');
        $this->addSql('DROP SEQUENCE refresh_tokens_id_seq CASCADE');
        $this->addSql('DROP SEQUENCE specialization_id_seq CASCADE');
        $this->addSql('DROP SEQUENCE "user_id_seq" CASCADE');
        $this->addSql('ALTER TABLE doctor DROP CONSTRAINT FK_1FC0F36AA76ED395');
        $this->addSql('ALTER TABLE doctor_specialization DROP CONSTRAINT FK_1187285D87F4FB17');
        $this->addSql('ALTER TABLE doctor_specialization DROP CONSTRAINT FK_1187285DFA846217');
        $this->addSql('ALTER TABLE doctor_disease DROP CONSTRAINT FK_9C96096C87F4FB17');
        $this->addSql('ALTER TABLE doctor_disease DROP CONSTRAINT FK_9C96096CD8355341');
        $this->addSql('ALTER TABLE image DROP CONSTRAINT FK_C53D045FA76ED395');
        $this->addSql('ALTER TABLE manager DROP CONSTRAINT FK_FA2425B9A76ED395');
        $this->addSql('ALTER TABLE patient DROP CONSTRAINT FK_1ADAD7EBF5B7AF75');
        $this->addSql('ALTER TABLE patient DROP CONSTRAINT FK_1ADAD7EBA76ED395');
        $this->addSql('DROP TABLE address');
        $this->addSql('DROP TABLE disease');
        $this->addSql('DROP TABLE doctor');
        $this->addSql('DROP TABLE doctor_specialization');
        $this->addSql('DROP TABLE doctor_disease');
        $this->addSql('DROP TABLE image');
        $this->addSql('DROP TABLE manager');
        $this->addSql('DROP TABLE patient');
        $this->addSql('DROP TABLE refresh_tokens');
        $this->addSql('DROP TABLE specialization');
        $this->addSql('DROP TABLE "user"');
    }
}
