import csv
import os
import numpy
from tabulate import tabulate


MAX_FILE_SIZE_MB = 1024


class Statistics:
    def __init__(self, min: float, max: float, median: float, mean: float):
        self.min = min
        self.max = max
        self.median = median
        self.mean = mean


class FileExtensionError(ValueError):
    pass


class FileSizeError(ValueError):
    pass


class ColumnNumberError(ValueError):
    pass


class FieldMismatchError(ValueError):
    pass


class RegionNumberError(ValueError):
    pass


class EmptyFieldError(ValueError):
    pass


def validate_file(filename: str) -> None:
    if not filename.endswith('.csv'):
        raise FileExtensionError
    elif os.path.getsize(filename) == 0:
        raise FileSizeError('File is empty')
    elif os.path.getsize(filename) > MAX_FILE_SIZE_MB * 1024 * 1024:
        raise FileSizeError('File is too large')


def read_column_number(column_count: int) -> int:
    try:
        column_number = int(input())
        if column_number >= column_count or column_number < 0:
            raise ColumnNumberError
    except ValueError:
        raise ColumnNumberError

    return column_number


def read_region_number(region_count: int) -> int:
    try:
        region_number = int(input())
        if region_number < 0 or region_number >= region_count:
            raise RegionNumberError
    except ValueError:
        raise RegionNumberError

    return region_number


def validate_row(row: list[str], column_count: int) -> list[str]:
    if len(row) != column_count:
        raise FieldMismatchError

    return row


def extract_column_values(rows: list[list[str]], column_number: int, column_count: int) -> list[float]:
    values = []
    for row in rows:
        validate_row(row, column_count)
        try:
            if not row[column_number]:
                raise EmptyFieldError
            value = float(row[column_number])
        except EmptyFieldError:
            print(f'Error: Empty field')
            continue
        except ValueError:
            print(f'Error: Invalid value "{row[column_number]}"')
            continue
        values.append(value)

    if not values:
        raise ValueError('No valid values found')

    return values


def extract_unique_sorted_regions(rows: list[list[str]]) -> list[str]:
    unique_regions = sorted({row[1] for row in rows[1:]})
    
    return unique_regions


def validate_rows(rows, column_count: int) -> None:
    for row in rows:
        validate_row(row, column_count)


def to_region_filtered_rows(region_number: int, rows: list[list[str]]) -> list[list[str]]:
    unique_regions = extract_unique_sorted_regions(rows)

    if region_number >= len(unique_regions) or region_number < 0:
        raise RegionNumberError(f"Region with index {region_number} doesn't exist")
    
    selected_region = unique_regions[region_number]
    
    filtered_rows = list(filter(lambda row: row[1] == selected_region, rows))
    
    if not filtered_rows:
        raise RegionNumberError(f"No rows found for region: {selected_region}")

    return filtered_rows

def calculate_statistics(values: list[float]) -> Statistics:
    values.sort()

    min = values[0]
    max = values[-1]
    median = (values[len(values) // 2] if len(values) % 2 == 1
        else (values[len(values) // 2] + values[len(values) // 2 - 1]) / 2)
    mean = sum(values) / len(values)

    return Statistics(min=min, max=max, median=median, mean=mean)


def print_rows(rows: list[list[str]], header: list[str]) -> None:
    print(tabulate(rows, headers=header, tablefmt='fancy_grid'))


def print_statistics(stat: Statistics) -> None:
    print(tabulate(
        [['Min', stat.min],
         ['Max', stat.max],
         ['Median', stat.median],
         ['Mean', stat.mean]],
         headers=['Statistics', 'Value']))
    print('\n')


def print_perecentile(values: list[float]) -> None:
    lst = [[f'{i}%', numpy.percentile(values, i)] for i in range(0, 101, 5)]
    print(tabulate(lst, headers=['Percentile', 'Value']))   


def print_regions_with_indices(regions: list[str]) -> None:
    indexed_regions = [(index, region) for index, region in enumerate(regions)]
    print(tabulate(indexed_regions, headers=["Index", "Region"], tablefmt='grid'))


def print_columns_with_indices(header: list[str]) -> None:
    indexed_columns = [(index, column) for index, column in enumerate(header)]
    print(tabulate(indexed_columns, headers=["Index", "Column"], tablefmt='grid'))    


def read_file(filename: str) -> list[list[str]]:
    with open(filename, 'r', encoding='utf-8') as file:
        validate_file(filename)

        rows = [row for row in csv.reader(file)]
        header = rows[0]
        column_count = len(header)

        validate_rows(rows, column_count)

        return rows


def select_region_number(rows: list[list[str]]) -> int:
    unique_regions = extract_unique_sorted_regions(rows)

    print_regions_with_indices(unique_regions)
    print('Region No: ')

    return read_region_number(len(unique_regions))


def select_column_number(rows: list[list[str]]) -> int:
    header = rows[0]
    column_count = len(header)

    print_columns_with_indices(header)
    print('Column No: ')

    return read_column_number(column_count)


def process_statistics(rows: list[list[str]], region_number: int, column_number: int) -> None:
    filtered_rows = to_region_filtered_rows(region_number, rows)
    header = rows[0]
    column_count = len(header)
    values = extract_column_values(filtered_rows, column_number, column_count)
    stat = calculate_statistics(values)

    print_rows(filtered_rows, header)
    print_statistics(stat)
    print_perecentile(values)
 

def main():
    print('CSV file name: ')

    filename = input()

    try:
        rows = read_file(filename)
        region_number = select_region_number(rows)
        column_number = select_column_number(rows)

        process_statistics(rows, region_number, column_number)

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
    except RegionNumberError as e:
        print(f'Error: Invalid region number')
    except Exception as e:
        print(f'Error: {e}')


if __name__ == '__main__':
    main()