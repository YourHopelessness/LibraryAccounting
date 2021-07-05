insert into roles values(0, 'debug', 'for debug');
insert into employees values (uuid_nil(), 'Dev', 'Dev', 'Developer', 'jiraffica@gmail.com', 'jiraffica@gmail.com', '+9 999 9999 99');
insert into user_roles values(uuid_nil(), 0);

insert into employees values ('11111111-1111-1111-1111-111111111111', 'Dev-adm', 'Dev-adm', 'Admin', 'nefilimii@list.ru', 'nefilimii@list.ru', '+9 999 9999 99');
insert into user_roles values('11111111-1111-1111-1111-111111111111', 1);

insert into employees values ('22222222-2222-2222-2222-222222222222', 'Dev-usr', 'Dev-usr', 'User', 'nmatyuhin@yandex.ru', 'nmatyuhin@yandex.ru', '+9 999 9999 99');
insert into user_roles values('22222222-2222-2222-2222-222222222222', 2);