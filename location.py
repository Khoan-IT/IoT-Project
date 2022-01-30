from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.common.by import By
import time
import os

path = os.getcwd()

option = webdriver.ChromeOptions()
option.add_argument('headless')
service = Service(executable_path='{}/chromedriver'.format(path))
browser = webdriver.Chrome(service=service, options=option)

def get_location():

    #Truy cập trang web my-location.org để lấy tạo độ hiện tại
    browser.get('https://www.google.com/maps')

    #Chờ 10s để tìm vị trí
    time.sleep(5)
    browser.find_element(By.ID, 'pWhrzc-mylocation').click()

    #Lấy tạo độ kinh tuyến và vĩ tuyến
    
    url = browser.current_url
    latitude = float(url.split(',')[0].split('@')[-1])
    longitude = float(url.split(',')[1])
    return latitude, longitude

print(get_location())

