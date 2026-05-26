Feature: UnitOfMeasure
    As a user in the Ecuador commercial sector
    I want to manage units of measure with their standard conversion factors
    So that I can accurately convert quantities for inventory and sales

    Scenario Outline: Verify Ecuador standard conversion factors for mass
        Given the unit of measure "<Unit>" with abbreviation "<Abbreviation>"
        When I check the conversion factor for "<Abbreviation>"
        Then the factor should be <Factor> relative to grams

        Examples:
            | Unit    | Abbreviation | Factor  |
            | Gramo   | g            | 1.0     |
            | Libra   | lb           | 454.0   |
            | Arroba  | @            | 11350.0 |
            | Quintal | qq           | 45400.0 |
            | Kilo    | kg           | 1000.0  |

    Scenario Outline: Verify Ecuador standard conversion factors for volume
        Given the unit of measure "<Unit>" with abbreviation "<Abbreviation>"
        When I check the conversion factor for "<Abbreviation>"
        Then the factor should be <Factor> relative to milliliters

        Examples:
            | Unit      | Abbreviation | Factor |
            | Mililitro | ml           | 1.0    |
            | Litro     | L            | 1000.0 |

    Scenario: Verify base unit for discrete items
        Given the unit of measure "Unidad" with abbreviation "u"
        When I check the conversion factor for "u"
        Then the factor should be 1.0 relative to units

    Scenario: Convert quantity from Libra to Grams
        Given a quantity of 2 "lb"
        When I convert the quantity to "g"
        Then the result should be 908.0

    Scenario: Convert quantity from Quintal to Grams
        Given a quantity of 0.5 "qq"
        When I convert the quantity to "g"
        Then the result should be 22700.0

    Scenario: Prevent conversion between incompatible types (Mass to Volume)
        Given a quantity of 1 "kg"
        When I try to convert the quantity to "ml"
        Then the conversion should fail
        And the error message should be "Incompatibilidad de unidades: no se puede convertir masa a volumen"

    Scenario: Handle rounding precision in conversions
        Given the unit of measure "Libra" with abbreviation "lb" has factor 454.0
        And a quantity of 1 "g"
        When I convert the quantity to "lb"
        Then the result should be 0.002203 with a precision of 6 decimal places

    Scenario: Retrieve only active units of measure
        Given the unit of measure "Gramos" with abbreviation "g"
        And the unit of measure "Antigua Libra" with abbreviation "alb" is inactive
        When I request all active units of measure
        Then the list should contain "g"
        And the list should not contain "alb"
