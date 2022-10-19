# Тестовое задание для поступления в GoCloudCamp

Первые две задачи являются обязательными к выполнению, третья по желанию.

Результаты выполнения тестового задания будем ждать на почте **gocloudcamp@sbercloud.ru** до 6 ноября 23:59:59.

## Вопросы для разогрева

1. Опишите самую интересную задачу в программировании, которую вам приходилось решать?
2. Расскажите о своем самом большом факапе? Что вы предприняли для решения проблемы?
3. Каковы ваши ожидания от участия в буткемпе?

---

## Distributed config

### Описание

Реализовать сервис, который позволит динамически управлять конфигурацией приложений. Доступ к сервису должен осуществляться с помощью API. Конфигурация может храниться в любом источнике данных, будь то файл на диске, либо база данных. Для удобства интеграции с сервисом может быть реализована клиентская библиотека.

### Технические требования

* реализация задания может быть выполнена на любом языке программирования
* формат схемы конфига на выбор: json/protobuf
* сервис должен обеспечивать персистентность данных
* сервис должен поддерживать все CRUD операции по работе с конфигом
* должно поддерживаться версионирование конфига при его изменении
* удалять конфиг допускается только если он не используется никаким приложением

### Будет здорово, если:
* в качестве протокола взаимодействия сервиса с клиентами будете использовать gRPC
* напишите Dockerfile и docker-compose.yml
* покроете проект unit-тестами
* сделаете тестовый пример использования написанного сервиса конфигураций

### Пример использования сервиса

#### Создание конфига

`curl -d "@data.json" -H "Content-Type: application/json" -X POST http://localhost:8080/config`

```json
{
    "service": "managed-k8s",
    "data": [
        {"key1": "value1"},
        {"key2": "value2"}
    ]
}
```

#### Получение конфига

`curl http://localhost:8080/config?service=managed-k8s`

```json
{"key1": "value1", "key2": "value2"}
```

---

## Предложить улучшения по go-vcloud-director

Что бы вы улучшили в данной библиотеке — https://github.com/vmware/go-vcloud-director?

Опишите ваши предложения в удобной для вас форме и прикрепите ссылку решения.