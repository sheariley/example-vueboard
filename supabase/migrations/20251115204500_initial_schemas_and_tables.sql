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


  create table "user_data"."projects" (
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


alter table "user_data"."projects" enable row level security;


  create table "user_data"."project_columns" (
    "id" integer generated always as identity not null,
    "uid" uuid not null default gen_random_uuid(),
    "created" timestamp without time zone default CURRENT_TIMESTAMP,
    "updated" timestamp without time zone default CURRENT_TIMESTAMP,
    "is_deleted" boolean default false,
    "project_id" integer not null,
    "name" character varying(32) not null,
    "fg_color" character varying(12),
    "bg_color" character varying(12),
    "index" integer not null default 0
      );


alter table "user_data"."project_columns" enable row level security;


  create table "user_data"."work_items" (
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
    "bg_color" character varying(12),
    "index" integer not null default 0
      );


alter table "user_data"."work_items" enable row level security;


  create table "user_data"."work_item_tags" (
    "id" integer generated always as identity not null,
    "uid" uuid not null default gen_random_uuid(),
    "user_id" uuid not null,
    "tag_text" character varying(30) not null
      );


alter table "user_data"."work_item_tags" enable row level security;


  create table "user_data"."work_item_tag_refs" (
    "work_item_tag_id" integer not null,
    "work_item_id" integer not null
      );


alter table "user_data"."work_item_tag_refs" enable row level security;

CREATE UNIQUE INDEX site_log_pkey ON site_data.site_log USING btree (id);

CREATE UNIQUE INDEX project_columns_pkey ON user_data.project_columns USING btree (id);

CREATE UNIQUE INDEX projects_pkey ON user_data.projects USING btree (id);

CREATE UNIQUE INDEX work_items_pkey ON user_data.work_items USING btree (id);

CREATE UNIQUE INDEX work_item_tags_pkey ON user_data.work_item_tags USING btree (id);

CREATE UNIQUE INDEX work_item_tag_refs_pkey ON user_data.work_item_tag_refs USING btree (work_item_tag_id, work_item_id);

alter table "site_data"."site_log" add constraint "site_log_pkey" PRIMARY KEY using index "site_log_pkey";

alter table "user_data"."projects" add constraint "projects_pkey" PRIMARY KEY using index "projects_pkey";

alter table "user_data"."project_columns" add constraint "project_columns_pkey" PRIMARY KEY using index "project_columns_pkey";

alter table "user_data"."work_items" add constraint "work_items_pkey" PRIMARY KEY using index "work_items_pkey";

alter table "user_data"."work_item_tags" add constraint "work_item_tags_pkey" PRIMARY KEY using index "work_item_tags_pkey";

alter table "user_data"."work_item_tag_refs" add constraint "work_item_tag_refs_pkey" PRIMARY KEY using index "work_item_tag_refs_pkey";

alter table "user_data"."projects" add constraint "projects_user_id_fkey" FOREIGN KEY (user_id) REFERENCES auth.users(id) not valid;

alter table "user_data"."projects" validate constraint "projects_user_id_fkey";

alter table "user_data"."project_columns" add constraint "project_columns_project_id_fkey" FOREIGN KEY (project_id) REFERENCES user_data.projects(id) ON DELETE CASCADE not valid;

alter table "user_data"."project_columns" validate constraint "project_columns_project_id_fkey";

alter table "user_data"."work_items" add constraint "work_items_project_column_id_fkey" FOREIGN KEY (project_column_id) REFERENCES user_data.project_columns(id) ON DELETE CASCADE not valid;

alter table "user_data"."work_items" validate constraint "work_items_project_column_id_fkey";

alter table "user_data"."work_item_tags" add constraint "work_item_tags_user_id_fkey" FOREIGN KEY (user_id) REFERENCES auth.users(id) not valid;

alter table "user_data"."work_item_tags" validate constraint "work_item_tags_user_id_fkey";

alter table "user_data"."work_item_tag_refs" add constraint "work_item_tag_refs_work_item_id_fkey" FOREIGN KEY (work_item_id) REFERENCES user_data.work_items(id) ON DELETE CASCADE not valid;

alter table "user_data"."work_item_tag_refs" validate constraint "work_item_tag_refs_work_item_id_fkey";

alter table "user_data"."work_item_tag_refs" add constraint "work_item_tag_refs_work_item_tag_id_fkey" FOREIGN KEY (work_item_tag_id) REFERENCES user_data.work_item_tags(id) not valid;

alter table "user_data"."work_item_tag_refs" validate constraint "work_item_tag_refs_work_item_tag_id_fkey";

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

grant insert on table "user_data"."projects" to "anon";

grant select on table "user_data"."projects" to "anon";

grant update on table "user_data"."projects" to "anon";

grant insert on table "user_data"."project_columns" to "anon";

grant select on table "user_data"."project_columns" to "anon";

grant update on table "user_data"."project_columns" to "anon";

grant insert on table "user_data"."work_items" to "anon";

grant select on table "user_data"."work_items" to "anon";

grant update on table "user_data"."work_items" to "anon";

grant insert on table "user_data"."work_item_tags" to "anon";

grant select on table "user_data"."work_item_tags" to "anon";

grant insert on table "user_data"."work_item_tag_refs" to "anon";

grant select on table "user_data"."work_item_tag_refs" to "anon";

grant delete on table "user_data"."work_item_tag_refs" to "anon";


  create policy "Allow anon to write to site_log"
  on "site_data"."site_log"
  as permissive
  for insert
  to anon
with check (true);



  create policy "Allow anon to insert into projects"
  on "user_data"."projects"
  as permissive
  for insert
  to anon
with check ((((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (user_id)::text));



  create policy "Allow anon to read from projects"
  on "user_data"."projects"
  as permissive
  for select
  to anon
using ((((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (user_id)::text));



  create policy "Allow anon to update projects"
  on "user_data"."projects"
  as permissive
  for update
  to anon
with check ((((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (user_id)::text));



  create policy "Allow anon to insert into project_columns"
  on "user_data"."project_columns"
  as permissive
  for insert
  to anon
with check ((EXISTS ( SELECT 1
   FROM user_data.projects p
  WHERE ((p.id = project_columns.project_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));



  create policy "Allow anon to read from project_columns"
  on "user_data"."project_columns"
  as permissive
  for select
  to anon
using ((EXISTS ( SELECT 1
   FROM user_data.projects p
  WHERE ((p.id = project_columns.project_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));



  create policy "Allow anon to update project_columns"
  on "user_data"."project_columns"
  as permissive
  for update
  to anon
with check ((EXISTS ( SELECT 1
   FROM user_data.projects p
  WHERE ((p.id = project_columns.project_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));

  create policy "Allow anon to insert into work_items"
  on "user_data"."work_items"
  as permissive
  for insert
  to anon
with check ((EXISTS ( SELECT 1
   FROM (user_data.project_columns pc
     JOIN user_data.projects p ON ((pc.project_id = p.id)))
  WHERE ((pc.id = work_items.project_column_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));



  create policy "Allow anon to read from work_items"
  on "user_data"."work_items"
  as permissive
  for select
  to anon
using ((EXISTS ( SELECT 1
   FROM (user_data.project_columns pc
     JOIN user_data.projects p ON ((pc.project_id = p.id)))
  WHERE ((pc.id = work_items.project_column_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));



  create policy "Allow anon to update work_items"
  on "user_data"."work_items"
  as permissive
  for update
  to anon
with check ((EXISTS ( SELECT 1
   FROM (user_data.project_columns pc
     JOIN user_data.projects p ON ((pc.project_id = p.id)))
  WHERE ((pc.id = work_items.project_column_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));



  create policy "Allow anon to insert into work_item_tags"
  on "user_data"."work_item_tags"
  as permissive
  for insert
  to anon
with check ((((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (user_id)::text));



  create policy "Allow anon to read from work_item_tags"
  on "user_data"."work_item_tags"
  as permissive
  for select
  to anon
using ((((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (user_id)::text));



  create policy "Allow anon to insert into work_item_tag_refs"
  on "user_data"."work_item_tag_refs"
  as permissive
  for insert
  to anon
with check ((EXISTS ( SELECT 1
   FROM ((user_data.work_items wi
     JOIN user_data.project_columns pc ON ((wi.project_column_id = pc.id)))
     JOIN user_data.projects p ON ((pc.project_id = p.id)))
  WHERE ((wi.id = work_item_tag_refs.work_item_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));



  create policy "Allow anon to read from work_item_tag_refs"
  on "user_data"."work_item_tag_refs"
  as permissive
  for select
  to anon
using ((EXISTS ( SELECT 1
   FROM ((user_data.work_items wi
     JOIN user_data.project_columns pc ON ((wi.project_column_id = pc.id)))
     JOIN user_data.projects p ON ((pc.project_id = p.id)))
  WHERE ((wi.id = work_item_tag_refs.work_item_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));



  create policy "Allow anon to delete from work_item_tag_refs"
  on "user_data"."work_item_tag_refs"
  as permissive
  for delete
  to anon
using ((EXISTS ( SELECT 1
   FROM ((user_data.work_items wi
     JOIN user_data.project_columns pc ON ((wi.project_column_id = pc.id)))
     JOIN user_data.projects p ON ((pc.project_id = p.id)))
  WHERE ((wi.id = work_item_tag_refs.work_item_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));


