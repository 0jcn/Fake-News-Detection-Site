CREATE SCHEMA IF NOT EXISTS fake_news_site;

CREATE TABLE IF NOT EXISTS fake_news_site.users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    tier VARCHAR(10) CHECK (tier IN ('User', 'Admin')) NOT NULL DEFAULT 'User',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS fake_news_site.saved_detections (
    detection_id SERIAL PRIMARY KEY,
    user_id INTEGER REFERENCES fake_news_site.users(id) ON DELETE CASCADE,
    input TEXT NOT NULL,
    result TEXT NOT NULL,
    true_prob NUMERIC,
    barely_true_prob NUMERIC,
    half_true_prob NUMERIC,
    false_prob NUMERIC,
    mostly_true_prob NUMERIC,
    pants_fire_prob NUMERIC,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);