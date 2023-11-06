<?php

namespace App\Constant;

abstract class ValidationConstants
{
    const BLANK_VALUE = 'This value should not be blank.';
    const INCORRECT_ID = 'must be an integer and greater than 0';
    const INCORRECT_STATUS = "Available value: 'ACTIVE' or 'DISABLED'";
    const INCORRECT_START_TIME = 'Incorrect time format. Available: (07:00 to 16:00)';
    const INCORRECT_END_TIME = 'Incorrect time format. Available: (08:00 to 17:00)';
    const INCORRECT_INTERVAL = 'Incorrect interval. Available: 1H or 15M or 30M or 45M';
    const INCORRECT_WORKDAYS = 'Incorrect workdays array. Available: [1, 2, 3, 4, 5, 6, 7]';
    const INVALID_EMAIL = 'This value is not a valid email address.';
    const INVALID_PASSWORD = 'The password must contain at least one number, one letter and one character';
    const INVALID_PESEL = 'PESEL must be a number';
    const INVALID_DATE = 'Invalid date. Please select a future date or date should not be later than +1 month.';
    const INVALID_DATE_OF_BIRTH = 'DateOfBirth must be Y-m-d (1999-12-31)';
    const INVALID_DATE_FORMAT = 'Invalid format. Date must be Y-m-d (1999-12-31)';
    const INVALID_TIME = 'Incorrect time format (07:00|15|30|45 to 16:00|15|30|45)';
    const INVALID_POSTAL_CODE = 'Valid postal code format: 00-000';
    const INVALID_PHONE_NUMBER = 'Phone number should start with + (optional) and contain only digits.';
    const INVALID_LENGTH_11 = 'This value should have exactly 11 characters.';
    const LONG_VALUE_5 = 'This value is too long. It should have 5 characters or less.';
    const LONG_VALUE_50 = 'This value is too long. It should have 50 characters or less.';
    const SHORT_VALUE_10 = 'This value is too short. It should have 10 characters or more.';
}