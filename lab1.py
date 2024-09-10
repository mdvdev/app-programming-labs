import csv
import os
import numpy
from tabulate import tabulate
from typing import TextIO


MAX_FILE_SIZE_MB = 1024


class FileExtensionError(ValueError):
    pass


class FileSizeError(ValueError):
    pass


class ColumnNumberError(ValueError):
    pass


class FieldMismatchError(ValueError):
    pass


class RegionNotExistsError(ValueError):
    pass


def validate_file(filename: str) -> None:
    if not filename.endswith('.csv'):
        raise FileExtensionError
    elif os.path.getsize(filename) == 0:
        raise FileSizeError('File is empty')
    elif os.path.getsize(filename) > MAX_FILE_SIZE_MB * 1024 * 1024:
        raise FileSizeError('File is too large')


def read_column_number() -> int:
    try:
        column_number = int(input())
    except ValueError:
        raise ColumnNumberError

    return column_number


def validate_column_number(column_number: int, column_count: int) -> int:
    if column_number >= column_count or column_number < 0:
        raise ColumnNumberError

    return column_number


def validate_row(row: list[str], column_count: int) -> list[str]:
    if len(row) != column_count:
        raise FieldMismatchError

    return row


def extract_column_values(rows: list[list[str]], column_number: int, column_count: int) -> list[float]:
    values = []
    for row in rows:
        validate_row(row, column_count)
        if not row[column_number]:
            continue
        try:
            value = float(row[column_number])
        except ValueError:
            print(f'Error: Invalid value "{row[column_number]}"')
            continue
        values.append(value)

    return values


def extract_rows(file: TextIO, column_count: int) -> list[list[str]]:
    rows = []
    for row in csv.reader(file):
        validate_row(row, column_count)
        rows.append(row)

    return rows


def to_region_filtered_rows(region: str, rows: list[list[str]]) -> list[list[str]]:
    filtered_rows = list(filter(lambda row: row[1] == region, rows))
    if not filtered_rows:
        raise RegionNotExistsError(region)

    return filtered_rows


def calculate_statistics(values: list[float]) -> tuple[float, float, float, float]:
    values.sort()

    min = values[0]
    max = values[-1]
    median = (values[len(values) // 2] if len(values) % 2 == 1
        else (values[len(values) // 2] + values[len(values) // 2 - 1]) / 2)
    mean = sum(values) / len(values)

    return min, max, median, mean


def print_rows(rows: list[list[str]], header: list[str]) -> None:
    print(tabulate(rows, headers=header, tablefmt='fancy_grid'))


def print_statistics(min: float, max: float, median: float, mean: float) -> None:
    print(tabulate(
        [['Min', min],
         ['Max', max],
         ['Median', median],
         ['Mean', mean]],
         headers=['Statistics', 'Value']))
    print('\n')


def print_perecentile(values: list[float]) -> None:
    lst = [[f'{i}%', numpy.percentile(values, i)] for i in range(0, 101, 5)]
    print(tabulate(lst, headers=['Percentile', 'Value']))   


print('CSV file name: ')

filename = input()

try:
    with open(filename, 'r', encoding='utf-8') as file:
        validate_file(filename)

        print('Region name: ')

        region = input()

        print('Column No: ')

        column_number = read_column_number()
        header = csv.reader(file).__next__()
        column_count = len(header)

        validate_column_number(column_number, column_count)

        filtered_rows = to_region_filtered_rows(region, extract_rows(file, column_count))
        values = extract_column_values(filtered_rows, column_number, column_count)
        min, max, median, mean = calculate_statistics(values)

        print_rows(filtered_rows, header)
        print_statistics(min, max, median, mean)
        print_perecentile(values)

except FileNotFoundError:
    print('Error: File not found')
except PermissionError:
    print('Error: Permission denied')
except FileExtensionError:
    print('Error: Invalid file extension')
except FileSizeError as e:
    print(f'Error: {e}')
except ColumnNumberError:
    print('Error: Invalid column number')
except FieldMismatchError:
    print('Error: Field mismatch')
except RegionNotExistsError as e:
    print(f'Error: Region "{e}" doesn\'t exists')
except Exception as e:
    print(f'Error: {e}')