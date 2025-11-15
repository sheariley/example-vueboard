create schema if not exists "const_data";

create schema if not exists "site_data";

create schema if not exists "user_data";


  create table "site_data"."site_log" (
    "id" integer generated always as identity not null,
    "created" timestamp without time zone not null default CURRENT_TIMESTAMP,
    "severity" character varying(8) not null,
    "message" character varying(2048),
    "context_data" jsonb,
    "client_ip" character varying(42)
      );


alter table "site_data"."site_log" enable row level security;


  create table "user_data"."project" (
    "id" integer generated always as identity not null,
    "uid" uuid not null default gen_random_uuid(),
    "created" timestamp without time zone default CURRENT_TIMESTAMP,
    "updated" timestamp without time zone default CURRENT_TIMESTAMP,
    "is_deleted" boolean default false,
    "user_id" uuid not null,
    "title" character varying(200) not null,
    "description" character varying(600),
    "default_card_fg_color" character varying(12),
    "default_card_bg_color" character varying(12)
      );


alter table "user_data"."project" enable row level security;


  create table "user_data"."project_column" (
    "id" integer generated always as identity not null,
    "uid" uuid not null default gen_random_uuid(),
    "created" timestamp without time zone default CURRENT_TIMESTAMP,
    "updated" timestamp without time zone default CURRENT_TIMESTAMP,
    "is_deleted" boolean default false,
    "project_id" integer not null,
    "name" character varying(32) not null,
    "fg_color" character varying(12),
    "bg_color" character varying(12)
      );


alter table "user_data"."project_column" enable row level security;


  create table "user_data"."work_item" (
    "id" integer generated always as identity not null,
    "uid" uuid not null default gen_random_uuid(),
    "created" timestamp without time zone default CURRENT_TIMESTAMP,
    "updated" timestamp without time zone default CURRENT_TIMESTAMP,
    "is_deleted" boolean default false,
    "project_column_id" integer not null,
    "title" character varying(200) not null,
    "description" character varying(600),
    "notes" text,
    "fg_color" character varying(12),
    "bg_color" character varying(12)
      );


alter table "user_data"."work_item" enable row level security;


  create table "user_data"."work_item_tag" (
    "id" integer generated always as identity not null,
    "uid" uuid not null default gen_random_uuid(),
    "user_id" uuid not null,
    "tag_text" character varying(30) not null
      );


alter table "user_data"."work_item_tag" enable row level security;


  create table "user_data"."work_item_tag_ref" (
    "work_item_tag_id" integer not null,
    "work_item_id" integer not null
      );


alter table "user_data"."work_item_tag_ref" enable row level security;

CREATE UNIQUE INDEX site_log_pkey ON site_data.site_log USING btree (id);

CREATE UNIQUE INDEX project_column_pkey ON user_data.project_column USING btree (id);

CREATE UNIQUE INDEX project_pkey ON user_data.project USING btree (id);

CREATE UNIQUE INDEX work_item_pkey ON user_data.work_item USING btree (id);

CREATE UNIQUE INDEX work_item_tag_pkey ON user_data.work_item_tag USING btree (id);

CREATE UNIQUE INDEX work_item_tag_ref_pkey ON user_data.work_item_tag_ref USING btree (work_item_tag_id, work_item_id);

alter table "site_data"."site_log" add constraint "site_log_pkey" PRIMARY KEY using index "site_log_pkey";

alter table "user_data"."project" add constraint "project_pkey" PRIMARY KEY using index "project_pkey";

alter table "user_data"."project_column" add constraint "project_column_pkey" PRIMARY KEY using index "project_column_pkey";

alter table "user_data"."work_item" add constraint "work_item_pkey" PRIMARY KEY using index "work_item_pkey";

alter table "user_data"."work_item_tag" add constraint "work_item_tag_pkey" PRIMARY KEY using index "work_item_tag_pkey";

alter table "user_data"."work_item_tag_ref" add constraint "work_item_tag_ref_pkey" PRIMARY KEY using index "work_item_tag_ref_pkey";

alter table "user_data"."project" add constraint "project_user_id_fkey" FOREIGN KEY (user_id) REFERENCES auth.users(id) not valid;

alter table "user_data"."project" validate constraint "project_user_id_fkey";

alter table "user_data"."project_column" add constraint "project_column_project_id_fkey" FOREIGN KEY (project_id) REFERENCES user_data.project(id) ON DELETE CASCADE not valid;

alter table "user_data"."project_column" validate constraint "project_column_project_id_fkey";

alter table "user_data"."work_item" add constraint "work_item_project_column_id_fkey" FOREIGN KEY (project_column_id) REFERENCES user_data.project_column(id) ON DELETE CASCADE not valid;

alter table "user_data"."work_item" validate constraint "work_item_project_column_id_fkey";

alter table "user_data"."work_item_tag" add constraint "work_item_tag_user_id_fkey" FOREIGN KEY (user_id) REFERENCES auth.users(id) not valid;

alter table "user_data"."work_item_tag" validate constraint "work_item_tag_user_id_fkey";

alter table "user_data"."work_item_tag_ref" add constraint "work_item_tag_ref_work_item_id_fkey" FOREIGN KEY (work_item_id) REFERENCES user_data.work_item(id) ON DELETE CASCADE not valid;

alter table "user_data"."work_item_tag_ref" validate constraint "work_item_tag_ref_work_item_id_fkey";

alter table "user_data"."work_item_tag_ref" add constraint "work_item_tag_ref_work_item_tag_id_fkey" FOREIGN KEY (work_item_tag_id) REFERENCES user_data.work_item_tag(id) not valid;

alter table "user_data"."work_item_tag_ref" validate constraint "work_item_tag_ref_work_item_tag_id_fkey";

set check_function_bodies = off;

CREATE OR REPLACE FUNCTION public.create_site_log_entry(severity character varying, message character varying, context_data jsonb, client_ip character varying DEFAULT NULL::character varying)
 RETURNS boolean
 LANGUAGE plpgsql
 SET search_path TO ''
AS $function$
BEGIN
  INSERT INTO site_data.site_log
  ("severity", "message", "context_data", "client_ip")
  VALUES
  (severity, message, context_data, client_ip);
  RETURN TRUE;
END;
$function$
;

grant insert on table "site_data"."site_log" to "anon";

grant insert on table "user_data"."project" to "anon";

grant select on table "user_data"."project" to "anon";

grant update on table "user_data"."project" to "anon";

grant insert on table "user_data"."project_column" to "anon";

grant select on table "user_data"."project_column" to "anon";

grant update on table "user_data"."project_column" to "anon";

grant insert on table "user_data"."work_item" to "anon";

grant select on table "user_data"."work_item" to "anon";

grant update on table "user_data"."work_item" to "anon";

grant insert on table "user_data"."work_item_tag" to "anon";

grant select on table "user_data"."work_item_tag" to "anon";

grant insert on table "user_data"."work_item_tag_ref" to "anon";

grant select on table "user_data"."work_item_tag_ref" to "anon";


  create policy "Allow anon to write to site_log"
  on "site_data"."site_log"
  as permissive
  for insert
  to anon
with check (true);



  create policy "Allow anon to insert into project"
  on "user_data"."project"
  as permissive
  for insert
  to anon
with check (true);



  create policy "Allow anon to read from project"
  on "user_data"."project"
  as permissive
  for select
  to anon
using (true);



  create policy "Allow anon to update project"
  on "user_data"."project"
  as permissive
  for update
  to anon
with check (true);



  create policy "Allow anon to insert into project_column"
  on "user_data"."project_column"
  as permissive
  for insert
  to anon
with check (true);



  create policy "Allow anon to read from project_column"
  on "user_data"."project_column"
  as permissive
  for select
  to anon
using (true);



  create policy "Allow anon to update project_column"
  on "user_data"."project_column"
  as permissive
  for update
  to anon
with check (true);



  create policy "Allow anon to insert into work_item"
  on "user_data"."work_item"
  as permissive
  for insert
  to anon
with check (true);



  create policy "Allow anon to read from work_item"
  on "user_data"."work_item"
  as permissive
  for select
  to anon
using (true);



  create policy "Allow anon to update work_item"
  on "user_data"."work_item"
  as permissive
  for update
  to anon
with check (true);



  create policy "Allow anon to insert into work_item_tag"
  on "user_data"."work_item_tag"
  as permissive
  for insert
  to anon
with check (true);



  create policy "Allow anon to read from work_item_tag"
  on "user_data"."work_item_tag"
  as permissive
  for select
  to anon
using (true);



  create policy "Allow anon to insert into work_item_tag_ref"
  on "user_data"."work_item_tag_ref"
  as permissive
  for insert
  to anon
with check (true);



  create policy "Allow anon to read from work_item_tag_ref"
  on "user_data"."work_item_tag_ref"
  as permissive
  for select
  to anon
using (true);



