FROM postgres:latest

ENV POSTGRES_USER=comp3000
ENV POSTGRES_PASSWORD=comp3000
ENV POSTGRES_DB=fakenewsdb

COPY init.sql /docker-entrypoint-initdb.d/init.sql

EXPOSE 5432