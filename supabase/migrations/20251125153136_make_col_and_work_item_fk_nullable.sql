alter table "user_data"."project_columns" alter column "project_id" drop not null;

alter table "user_data"."work_items" alter column "project_column_id" drop not null;


