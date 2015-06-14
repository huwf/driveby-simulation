
'''
Will be useful for generating histograms...
'''
def write_case_statement(start, end, step):
	sql_case = 'SELECT\n'
	for i in range(start,end,step):			
		high = i + step
		sql_case += '\tCASE WHEN Average > %d AND Average <= %d THEN 1 ELSE 0 END AS "%d-%d",\n' % (i, high,i,high)
	sql_case = sql_case[:-2] + "\n"
	#print sql_case
	sql_case += 'FROM\n(\n\n)a\n)b'
	print sql_case
	return sql_case


def write_histogram_select_query(start,end,step):
	sql_count_string = 'SELECT\n'
	for i in range(start,end,step):
		high = i + step
		sql_count_string += '\tSUM("%d-%d") AS "%d-%d",\n' % (i,high,i,high)
	sql_count_string = sql_count_string[:-2] + "\nFROM\n(\n"
	print sql_count_string


write_histogram_select_query(0,100,1)
write_case_statement(0,100,1)
