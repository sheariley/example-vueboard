drop policy "Allow anon to insert into project" on "user_data"."project";

drop policy "Allow anon to read from project" on "user_data"."project";

drop policy "Allow anon to update project" on "user_data"."project";

drop policy "Allow anon to insert into project_column" on "user_data"."project_column";

drop policy "Allow anon to read from project_column" on "user_data"."project_column";

drop policy "Allow anon to update project_column" on "user_data"."project_column";

drop policy "Allow anon to insert into work_item" on "user_data"."work_item";

drop policy "Allow anon to read from work_item" on "user_data"."work_item";

drop policy "Allow anon to update work_item" on "user_data"."work_item";

drop policy "Allow anon to insert into work_item_tag" on "user_data"."work_item_tag";

drop policy "Allow anon to read from work_item_tag" on "user_data"."work_item_tag";

drop policy "Allow anon to insert into work_item_tag_ref" on "user_data"."work_item_tag_ref";

drop policy "Allow anon to read from work_item_tag_ref" on "user_data"."work_item_tag_ref";

  create policy "Allow anon to insert into project"
  on "user_data"."project"
  as permissive
  for insert
  to anon
with check ((((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (user_id)::text));



  create policy "Allow anon to read from project"
  on "user_data"."project"
  as permissive
  for select
  to anon
using ((((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (user_id)::text));



  create policy "Allow anon to update project"
  on "user_data"."project"
  as permissive
  for update
  to anon
with check ((((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (user_id)::text));



  create policy "Allow anon to insert into project_column"
  on "user_data"."project_column"
  as permissive
  for insert
  to anon
with check ((EXISTS ( SELECT 1
   FROM user_data.project p
  WHERE ((p.id = project_column.project_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));



  create policy "Allow anon to read from project_column"
  on "user_data"."project_column"
  as permissive
  for select
  to anon
using ((EXISTS ( SELECT 1
   FROM user_data.project p
  WHERE ((p.id = project_column.project_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));



  create policy "Allow anon to update project_column"
  on "user_data"."project_column"
  as permissive
  for update
  to anon
with check ((EXISTS ( SELECT 1
   FROM user_data.project p
  WHERE ((p.id = project_column.project_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));



  create policy "Allow anon to insert into work_item"
  on "user_data"."work_item"
  as permissive
  for insert
  to anon
with check ((EXISTS ( SELECT 1
   FROM (user_data.project_column pc
     JOIN user_data.project p ON ((pc.project_id = p.id)))
  WHERE ((pc.id = work_item.project_column_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));



  create policy "Allow anon to read from work_item"
  on "user_data"."work_item"
  as permissive
  for select
  to anon
using ((EXISTS ( SELECT 1
   FROM (user_data.project_column pc
     JOIN user_data.project p ON ((pc.project_id = p.id)))
  WHERE ((pc.id = work_item.project_column_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));



  create policy "Allow anon to update work_item"
  on "user_data"."work_item"
  as permissive
  for update
  to anon
with check ((EXISTS ( SELECT 1
   FROM (user_data.project_column pc
     JOIN user_data.project p ON ((pc.project_id = p.id)))
  WHERE ((pc.id = work_item.project_column_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));



  create policy "Allow anon to insert into work_item_tag"
  on "user_data"."work_item_tag"
  as permissive
  for insert
  to anon
with check ((((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (user_id)::text));



  create policy "Allow anon to read from work_item_tag"
  on "user_data"."work_item_tag"
  as permissive
  for select
  to anon
using ((((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (user_id)::text));



  create policy "Allow anon to insert into work_item_tag_ref"
  on "user_data"."work_item_tag_ref"
  as permissive
  for insert
  to anon
with check ((EXISTS ( SELECT 1
   FROM ((user_data.work_item wi
     JOIN user_data.project_column pc ON ((wi.project_column_id = pc.id)))
     JOIN user_data.project p ON ((pc.project_id = p.id)))
  WHERE ((wi.id = work_item_tag_ref.work_item_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));



  create policy "Allow anon to read from work_item_tag_ref"
  on "user_data"."work_item_tag_ref"
  as permissive
  for select
  to anon
using ((EXISTS ( SELECT 1
   FROM ((user_data.work_item wi
     JOIN user_data.project_column pc ON ((wi.project_column_id = pc.id)))
     JOIN user_data.project p ON ((pc.project_id = p.id)))
  WHERE ((wi.id = work_item_tag_ref.work_item_id) AND (((current_setting('request.jwt.claims'::text, true))::json ->> 'sub'::text) = (p.user_id)::text)))));



