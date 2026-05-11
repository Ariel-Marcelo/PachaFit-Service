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
