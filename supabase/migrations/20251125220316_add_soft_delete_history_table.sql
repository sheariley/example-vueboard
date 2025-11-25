
  create table "user_data"."soft_deletes" (
    "id" integer generated always as identity not null,
    "uid" uuid not null default gen_random_uuid(),
    "user_id" uuid not null,
    "deleted" timestamp without time zone default CURRENT_TIMESTAMP,
    "entity_type" character varying(64) not null,
    "parent_id" integer
      );


alter table "user_data"."soft_deletes" enable row level security;

CREATE UNIQUE INDEX soft_deletes_pkey ON user_data.soft_deletes USING btree (id);

alter table "user_data"."soft_deletes" add constraint "soft_deletes_pkey" PRIMARY KEY using index "soft_deletes_pkey";

alter table "user_data"."soft_deletes" add constraint "soft_deletes_user_id_fkey" FOREIGN KEY (user_id) REFERENCES auth.users(id) not valid;

alter table "user_data"."soft_deletes" validate constraint "soft_deletes_user_id_fkey";

grant delete on table "user_data"."soft_deletes" to "anon";

grant insert on table "user_data"."soft_deletes" to "anon";

grant select on table "user_data"."soft_deletes" to "anon";

grant update on table "user_data"."soft_deletes" to "anon";


  create policy "Allow anon to delete from soft_deletes"
  on "user_data"."soft_deletes"
  as permissive
  for delete
  to anon
using ((((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (user_id)::text));



  create policy "Allow anon to insert into soft_deletes"
  on "user_data"."soft_deletes"
  as permissive
  for insert
  to anon
with check ((((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (user_id)::text));



  create policy "Allow anon to read from soft_deletes"
  on "user_data"."soft_deletes"
  as permissive
  for select
  to anon
using ((((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (user_id)::text));



  create policy "Allow anon to update soft_deletes"
  on "user_data"."soft_deletes"
  as permissive
  for update
  to anon
with check ((((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (user_id)::text));



